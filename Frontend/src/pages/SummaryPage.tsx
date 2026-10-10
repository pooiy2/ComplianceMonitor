import { useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getSummary, type Summary } from "../api";
import RequestState from "../components/RequestState";

const categories = [
  { key: "complies", label: "Complies", color: "green", description: "Aligned with guidelines" },
  { key: "deviates", label: "Deviates", color: "red", description: "Needs attention" },
  { key: "unclear", label: "Unclear", color: "amber", description: "Needs a closer look" },
] as const;

function SummaryPage() {
  const [summary, setSummary] = useState<Summary | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadSummary = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      setSummary(await getSummary());
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : "An unexpected error occurred.");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadSummary();
  }, [loadSummary]);

  const total = summary?.total ?? 0;

  return (
    <section className="page">
      <div className="page-heading">
        <div>
          <p className="eyebrow">YOUR WORKSPACE <span className="eyebrow-line" /></p>
          <h1>Make sense of <span className="heading-accent">every action.</span></h1>
          <p className="page-intro">A clear view of how actions align with the guidelines that matter.</p>
        </div>
        <Link className="button button-primary" to="/analyze">
          <span className="button-plus">+</span> New analysis
        </Link>
      </div>

      <RequestState loading={loading} error={error} onRetry={() => void loadSummary()} />

      {!loading && !error && summary && (
        <>
          <div className="summary-layout">
            <section className="panel chart-panel" aria-labelledby="distribution-title">
              <div className="panel-heading">
                <div>
                  <p className="eyebrow">ANALYSIS SNAPSHOT</p>
                  <h2 id="distribution-title">Guideline alignment</h2>
                </div>
                <span className="total-pill">{total} total</span>
              </div>
              <div className="chart-area" role="img" aria-label={`Analysis results: ${summary.complies} complies, ${summary.deviates} deviates, ${summary.unclear} unclear`}>
                <div className="chart-gridlines"><span /><span /><span /><span /></div>
                <div className="bar-chart">
                  {categories.map((category) => {
                    const value = summary[category.key];
                    const height = total > 0 ? Math.max((value / total) * 100, value > 0 ? 6 : 0) : 0;
                    return (
                      <div className="bar-column" key={category.key}>
                        <span className="bar-count">{value}</span>
                        <div className="bar-track">
                          <div className={`bar-fill bar-${category.color}`} style={{ height: `${height}%` }} />
                        </div>
                        <span className="bar-label">{category.label}</span>
                      </div>
                    );
                  })}
                </div>
              </div>
              <div className="chart-legend">
                {categories.map((category) => (
                  <div className="legend-item" key={category.key}>
                    <span className={`legend-dot dot-${category.color}`} />
                    <span>{category.label}</span>
                    <span className="legend-detail">{category.description}</span>
                  </div>
                ))}
              </div>
            </section>

            <aside className="summary-aside">
              <article className="panel total-card">
                <span className="card-icon icon-total">↗</span>
                <p className="metric-label">TOTAL ANALYSES</p>
                <strong className="metric-value">{summary.total}</strong>
                <span className="metric-caption">all-time evaluations</span>
              </article>
              <article className="panel metric-card">
                <span className="card-icon icon-green">✓</span>
                <div>
                  <p className="metric-label">COMPLIES</p>
                  <strong className="metric-value">{summary.complies}</strong>
                </div>
                <span className="metric-caption">aligned</span>
              </article>
              <article className="panel metric-card">
                <span className="card-icon icon-amber">?</span>
                <div>
                  <p className="metric-label">UNCLEAR</p>
                  <strong className="metric-value">{summary.unclear}</strong>
                </div>
                <span className="metric-caption">review needed</span>
              </article>
              <article className="panel metric-card">
                <span className="card-icon icon-red">!</span>
                <div>
                  <p className="metric-label">DEVIATES</p>
                  <strong className="metric-value">{summary.deviates}</strong>
                </div>
                <span className="metric-caption">follow up</span>
              </article>
            </aside>
          </div>

          <section className="callout">
            <div className="callout-mark">✳</div>
            <div>
              <h2>Clarity starts with a single action.</h2>
              <p>Compare what happened against what should happen.</p>
            </div>
            <Link className="text-link" to="/analyze">Start an analysis <span aria-hidden="true">→</span></Link>
          </section>
        </>
      )}
    </section>
  );
}

export default SummaryPage;
