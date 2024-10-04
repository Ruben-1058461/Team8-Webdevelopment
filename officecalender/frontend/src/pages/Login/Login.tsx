import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "../../App.css";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  console.log("Login component is rendered"); // Test

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const response = await axios.post(
        "http://localhost:5002/api/auth/login",
        {
          email: email,
          password,
        },
        {
          headers: {
            "Content-Type": "application/json"
          }
        }
      );

      // Log the response for debugging
      console.log("Login response:", response);

      // Save token in localstorage
      localStorage.setItem("token", response.data.token);

      // Navigate to homepage or other
      navigate("/");
    } catch (error) {
      // Handle the error
      if (axios.isAxiosError(error)) {
        // Log specific error response if it's an Axios error
        console.error("Axios error message:", error.message);
        console.error("Axios error response data:", error.response?.data);
        console.error("Axios error response status:", error.response?.status);
        console.error("Axios error response headers:", error.response?.headers);
      } else {
        // Log any other error types
        console.error("Error logging in:", error);
      }
      
      // Display a generic message to the user
      alert("Login failed. Please check your credentials and try again.");
    }
  };

  return (
    <div className="login-container">
      <div className="login-box">
        <h2>Inloggen</h2>
        <form onSubmit={handleLogin}>
          <div>
            <label>Email:</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
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
