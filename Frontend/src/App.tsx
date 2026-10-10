import { NavLink, Route, Routes } from "react-router-dom";
import SummaryPage from "./pages/SummaryPage";
import AnalyzePage from "./pages/AnalyzePage";
import HistoryPage from "./pages/HistoryPage";

function App() {
  return (
    <div className="app-shell">
      <header className="topbar">
        <NavLink className="brand" to="/" aria-label="Analysis home">
          <span className="brand-mark">LK</span>
          <span>Luke Keating</span>
        </NavLink>
        <nav className="main-nav" aria-label="Main navigation">
          <NavLink end to="/" className={({ isActive }) => `nav-link${isActive ? " active" : ""}`}>
            Overview
          </NavLink>
          <NavLink to="/analyze" className={({ isActive }) => `nav-link${isActive ? " active" : ""}`}>
            Analyze
          </NavLink>
          <NavLink to="/history" className={({ isActive }) => `nav-link${isActive ? " active" : ""}`}>
            History
          </NavLink>
        </nav>
        <span className="topbar-note"><span className="status-dot" /> Action compliance</span>
      </header>

      <main className="main-content">
        <Routes>
          <Route path="/" element={<SummaryPage />} />
          <Route path="/analyze" element={<AnalyzePage />} />
          <Route path="/history" element={<HistoryPage />} />
          <Route path="*" element={<SummaryPage />} />
        </Routes>
      </main>

      <footer className="site-footer">
        <span>Analysis <span className="footer-separator">/</span> Action compliance</span>
        <span>Make every decision clearer.</span>
      </footer>
    </div>
  );
}

export default App;
