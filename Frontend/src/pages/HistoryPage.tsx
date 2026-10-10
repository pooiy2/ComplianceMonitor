import { useCallback, useEffect, useMemo, useState } from "react";
import { getHistory, normalizeResult, type HistoryRecord } from "../api";
import RequestState from "../components/RequestState";
import ResultBadge from "../components/ResultBadge";

type SortKey = "id" | "action" | "guideline" | "result" | "confidence" | "timestamp";
type SortDirection = "asc" | "desc";

const resultOrder = { complies: 0, deviates: 1, unclear: 2 };

function HistoryPage() {
  const [records, setRecords] = useState<HistoryRecord[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [query, setQuery] = useState("");
  const [resultFilter, setResultFilter] = useState("all");
  const [sortKey, setSortKey] = useState<SortKey>("timestamp");
  const [sortDirection, setSortDirection] = useState<SortDirection>("desc");

  const loadHistory = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      setRecords(await getHistory());
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : "An unexpected error occurred.");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadHistory();
  }, [loadHistory]);

  const visibleRecords = useMemo(() => {
    const normalizedQuery = query.trim().toLocaleLowerCase();
    return records
      .filter((record) => {
        const matchesQuery =
          !normalizedQuery ||
          record.action.toLocaleLowerCase().includes(normalizedQuery) ||
          record.guideline.toLocaleLowerCase().includes(normalizedQuery);
        const matchesResult = resultFilter === "all" || normalizeResult(record.result) === resultFilter;
        return matchesQuery && matchesResult;
      })
      .sort((left, right) => {
        let comparison: number;
        if (sortKey === "result") {
          comparison = resultOrder[normalizeResult(left.result)] - resultOrder[normalizeResult(right.result)];
        } else if (sortKey === "confidence" || sortKey === "id") {
          comparison = Number(left[sortKey]) - Number(right[sortKey]);
        } else if (sortKey === "timestamp") {
          comparison = new Date(left.timestamp).getTime() - new Date(right.timestamp).getTime();
        } else {
          comparison = String(left[sortKey] ?? "").localeCompare(String(right[sortKey] ?? ""));
        }
        return sortDirection === "asc" ? comparison : -comparison;
      });
  }, [query, records, resultFilter, sortDirection, sortKey]);

  function updateSort(key: SortKey) {
    if (sortKey === key) {
      setSortDirection((direction) => direction === "asc" ? "desc" : "asc");
    } else {
      setSortKey(key);
      setSortDirection(key === "timestamp" || key === "id" ? "desc" : "asc");
    }
  }

  function sortLabel(key: SortKey, label: string) {
    const indicator = sortKey === key ? (sortDirection === "asc" ? " ↑" : " ↓") : "";
    return <button className="sort-button" onClick={() => updateSort(key)} aria-label={`Sort by ${label}${indicator}`}>{label}{indicator}</button>;
  }

  return (
    <section className="page">
      <div className="page-heading">
        <div>
          <p className="eyebrow">YOUR RECORDS <span className="eyebrow-line" /></p>
          <h1>Every analysis, <span className="heading-accent">in one place.</span></h1>
          <p className="page-intro">Search, compare, and revisit previous action reviews.</p>
        </div>
        <div className="history-count"><strong>{records.length}</strong><span>total records</span></div>
      </div>

      <section className="panel history-panel">
        <div className="history-toolbar">
          <div className="search-field">
            <span aria-hidden="true" className="search-icon">⌕</span>
            <input
              type="search"
              aria-label="Search actions and guidelines"
              placeholder="Search actions or guidelines…"
              value={query}
              onChange={(event) => setQuery(event.target.value)}
            />
            {query && <button className="clear-search" onClick={() => setQuery("")} aria-label="Clear search">×</button>}
          </div>
          <label className="filter-select">
            <span>RESULT</span>
            <select value={resultFilter} onChange={(event) => setResultFilter(event.target.value)}>
              <option value="all">All results</option>
              <option value="complies">Complies</option>
              <option value="deviates">Deviates</option>
              <option value="unclear">Unclear</option>
            </select>
          </label>
          <button className="button button-secondary button-small refresh-button" onClick={() => void loadHistory()} disabled={loading}>
            <span aria-hidden="true">↻</span> Refresh
          </button>
        </div>

        <RequestState loading={loading} error={error} onRetry={() => void loadHistory()} />

        {!loading && !error && (
          <>
            <div className="results-summary">
              <span>Showing <strong>{visibleRecords.length}</strong> of {records.length} analyses</span>
              {(query || resultFilter !== "all") && <button className="reset-filters" onClick={() => { setQuery(""); setResultFilter("all"); }}>Clear filters</button>}
            </div>
            <div className="table-scroll">
              <table className="history-table">
                <thead>
                  <tr>
                    <th>{sortLabel("action", "ACTION")}</th>
                    <th>{sortLabel("guideline", "GUIDELINE")}</th>
                    <th>{sortLabel("result", "RESULT")}</th>
                    <th>{sortLabel("confidence", "CONFIDENCE")}</th>
                    <th>{sortLabel("timestamp", "DATE")}</th>
                  </tr>
                </thead>
                <tbody>
                  {visibleRecords.map((record) => (
                    <tr key={record.id}>
                      <td className="action-cell">{record.action}</td>
                      <td className="guideline-cell">{record.guideline}</td>
                      <td><ResultBadge result={record.result} /></td>
                      <td className="confidence-cell">{(record.confidence * 100).toFixed(1)}%</td>
                      <td className="date-cell">{new Date(record.timestamp).toLocaleString()}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
              {visibleRecords.length === 0 && (
                <div className="empty-state">
                  <span className="empty-icon">⌕</span>
                  <strong>No matching analyses</strong>
                  <p>Try another search term or clear your filters.</p>
                </div>
              )}
            </div>
          </>
        )}
      </section>
    </section>
  );
}

export default HistoryPage;
