interface RequestStateProps {
  loading: boolean;
  error: string | null;
  onRetry?: () => void;
}

export default function RequestState({ loading, error, onRetry }: RequestStateProps) {
  if (loading) {
    return (
      <div className="request-state" role="status">
        <span className="loading-spinner" />
        <span>Loading your data…</span>
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-state" role="alert">
        <div>
          <strong>We couldn’t load this data.</strong>
          <p>{error}</p>
        </div>
        {onRetry && <button className="button button-secondary button-small" onClick={onRetry}>Try again</button>}
      </div>
    );
  }

  return null;
}
