import { useState, type FormEvent } from "react";
import { analyzeAction, type AnalysisResponse } from "../api";
import ResultBadge from "../components/ResultBadge";

function AnalyzePage() {
  const [action, setAction] = useState("");
  const [guideline, setGuideline] = useState("");
  const [result, setResult] = useState<AnalysisResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError(null);
    setResult(null);
    setLoading(true);
    try {
      setResult(await analyzeAction({ action: action.trim(), guideline: guideline.trim() }));
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : "An unexpected error occurred.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <section className="page page-narrow">
      <div className="page-heading page-heading-stack">
        <div>
          <p className="eyebrow">NEW EVALUATION <span className="eyebrow-line" /></p>
          <h1>Put an action <span className="heading-accent">in context.</span></h1>
          <p className="page-intro">Describe what happened and the guideline it should be measured against.</p>
        </div>
      </div>

      <div className="analysis-layout">
        <form className="panel analysis-form" onSubmit={handleSubmit}>
          <div className="form-heading">
            <span className="step-number">01</span>
            <div><h2>Tell us what happened</h2><p>Both fields are needed for an accurate comparison.</p></div>
          </div>
          <label className="field-label" htmlFor="action">Action taken <span>Required</span></label>
          <textarea
            id="action"
            name="action"
            rows={4}
            value={action}
            onChange={(event) => setAction(event.target.value)}
            placeholder="Describe the action that was taken…"
            required
            maxLength={4000}
          />
          <div className="field-meta"><span>Be specific and include relevant details.</span><span>{action.length}/4000</span></div>

          <label className="field-label guideline-label" htmlFor="guideline">Relevant guideline <span>Required</span></label>
          <textarea
            id="guideline"
            name="guideline"
            rows={4}
            value={guideline}
            onChange={(event) => setGuideline(event.target.value)}
            placeholder="Add the policy, rule, or standard to compare against…"
            required
            maxLength={4000}
          />
          <div className="field-meta"><span>Paste the exact guideline when possible.</span><span>{guideline.length}/4000</span></div>

          {error && <div className="form-error" role="alert"><strong>Analysis could not be completed.</strong><span>{error}</span></div>}

          <button className="button button-primary submit-button" type="submit" disabled={loading || !action.trim() || !guideline.trim()}>
            {loading ? <><span className="button-spinner" /> Analyzing…</> : <>Analyze action <span aria-hidden="true">→</span></>}
          </button>
          <p className="privacy-note"><span aria-hidden="true">◈</span> Your analysis is saved to history.</p>
        </form>

        <aside className="analysis-aside">
          <div className="aside-illustration"><span className="orbit orbit-one" /><span className="orbit orbit-two" /><span className="orbit-core">e</span><span className="orbit-star">✳</span></div>
          <p className="eyebrow">HOW IT WORKS</p>
          <h2>Context makes the difference.</h2>
          <p>We compare an action with its guideline and returns a result with a confidence score to support your review.</p>
          <ul className="result-key">
            <li><span className="legend-dot dot-green" /><span><strong>Complies</strong> — aligned with the guideline</span></li>
            <li><span className="legend-dot dot-red" /><span><strong>Deviates</strong> — appears to differ from the guideline</span></li>
            <li><span className="legend-dot dot-amber" /><span><strong>Unclear</strong> — needs more context or review</span></li>
          </ul>
        </aside>
      </div>

      {result && (
        <section className="panel analysis-result" aria-live="polite">
          <div className="result-heading">
            <div><p className="eyebrow">ANALYSIS COMPLETE</p><h2>Here’s what we found.</h2></div>
            <ResultBadge result={result.result} />
          </div>
          <div className="result-comparison">
            <div><span className="result-field-label">ACTION</span><p>{result.action}</p></div>
            <span className="comparison-arrow">↔</span>
            <div><span className="result-field-label">GUIDELINE</span><p>{result.guideline}</p></div>
          </div>
          <div className="result-meta">
            <span>Confidence <strong>{(result.confidence * 100).toFixed(1)}%</strong></span>
            <span>Analyzed <strong>{new Date(result.timestamp).toLocaleString()}</strong></span>
          </div>
        </section>
      )}
    </section>
  );
}

export default AnalyzePage;
