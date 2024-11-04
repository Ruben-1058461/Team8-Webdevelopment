import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login/Login';
import Logout from './pages/Logout/Logout';
import Dashboard from './pages/Dashboard/Dashboard';

const App = () => {
  console.log('App component is rendered');
  return (
      <Router>
          <Routes>
          <Route path="/" element={<Navigate to="/login" />} />
              <Route path="/login" element={<Login />} /> 
              <Route path="/logout" element={<Logout />} />
              <Route path="/dashboard" element={<Dashboard />} />
              {/* Voeg hier andere routes toe */}
          </Routes>
      </Router>
  );
};

export default App;