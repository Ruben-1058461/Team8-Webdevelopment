import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';  // Zorg ervoor dat deze import correct is
import './index.css';     // CSS-bestanden indien nodig

const root = ReactDOM.createRoot(document.getElementById('root')!);
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
