import { normalizeResult, type HistoryResult } from "../api";

interface ResultBadgeProps {
  result: HistoryResult;
}

export default function ResultBadge({ result }: ResultBadgeProps) {
  const label = normalizeResult(result);
  return <span className={`result-badge result-${label}`}>{label}</span>;
}
