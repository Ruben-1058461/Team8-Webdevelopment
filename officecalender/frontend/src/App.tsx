import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Dashboard from './pages/Dashboard/Dashboard'; // Import your Dashboard component

const App = () => {
  return (
    <Router>
      <Routes>
        
        <Route path="/Dashboard" element={<Dashboard />} /> {/* Your Dashboard component */}
      </Routes>
    </Router>
  );
};

export default App;