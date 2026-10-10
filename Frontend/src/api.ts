export type AnalysisResult = "complies" | "deviates" | "unclear";

export interface Summary {
  total: number;
  complies: number;
  deviates: number;
  unclear: number;
}

export interface AnalysisRequest {
  action: string;
  guideline: string;
}

export interface AnalysisResponse extends AnalysisRequest {
  result: AnalysisResult;
  confidence: number;
  timestamp: string;
}

export type HistoryResult = number | AnalysisResult | "COMPLIES" | "DEVIATES" | "UNCLEAR";

export interface HistoryRecord {
  id: number;
  action: string;
  guideline: string;
  confidence: number;
  timestamp: string;
  result: HistoryResult;
}

const API_BASE_URL = (import.meta.env.VITE_API_BASE_URL ?? "https://localhost:7089").replace(/\/$/, "");

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...init,
    headers: {
      Accept: "application/json",
      ...init?.headers,
    },
  });

  if (!response.ok) {
    const detail = await response.text();
    throw new Error(
      detail ? `Request failed (${response.status}): ${detail}` : `Request failed (${response.status} ${response.statusText})`,
    );
  }

  return (await response.json()) as T;
}

export function getSummary(): Promise<Summary> {
  return request<Summary>("/Summary");
}

export function analyzeAction(payload: AnalysisRequest): Promise<AnalysisResponse> {
  return request<AnalysisResponse>("/Analyze", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
}

export function getHistory(): Promise<HistoryRecord[]> {
  return request<HistoryRecord[]>("/History");
}

export function normalizeResult(result: HistoryResult): AnalysisResult {
  if (typeof result === "number") {
    const numericResults: AnalysisResult[] = ["complies", "deviates", "unclear"];
    const normalized = numericResults[result];
    if (normalized) return normalized;
  } else {
    const normalized = result.toLowerCase();
    if (normalized === "complies" || normalized === "deviates" || normalized === "unclear") {
      return normalized;
    }
  }

  throw new Error(`Unsupported analysis result: ${String(result)}`);
}
