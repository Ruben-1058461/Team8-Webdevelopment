import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "../../App.css";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  console.log("Login component is rendered"); // Test

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const response = await axios.post(
        "http://localhost:5002/api/auth/login",
        {
          firstName: username,
          password,
        }
      );

      // Save token in localstorage
      localStorage.setItem("token", response.data.token);

      // Navigate to homepage or other
      navigate("/");
    } catch (error) {
      console.error("Error logging in", error);
    }
  };

  return (
    <div className="login-container">
      {" "}
      {/* Centrerende container */}
      <div className="login-box">
        {" "}
        {/* Hoofd box met styling */}
        <h2>Inloggen</h2>
        <form onSubmit={handleLogin}>
          <div>
            <label>Username:</label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <div>
            <label>Wachtwoord:</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <button type="submit">Inloggen</button>
        </form>
      </div>
    </div>
  );
};

export default Login;
