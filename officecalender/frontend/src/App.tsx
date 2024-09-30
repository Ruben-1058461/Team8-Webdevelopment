import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Login from './pages/Login/Login';
import Logout from './pages/Logout/Logout';

const App = () => {
  console.log('App component is rendered');
  return (
      <Router>
          <Routes>
              <Route path="/login" element={<Login />} /> 
              <Route path="/logout" element={<Logout />} />
              {/* Voeg hier andere routes toe */}
          </Routes>
      </Router>
  );
};

export default App;