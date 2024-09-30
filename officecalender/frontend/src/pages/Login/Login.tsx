import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  console.log('Login component is rendered'); // Test


  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

  //   try {
  //     const response = await axios.post('http://localhost:5002/api/login', {
  //       username,
  //       password,
  //     });

  //     // Save token in localstorage
  //     localStorage.setItem('token', response.data.token);

  //     // Navigate to homepage or other
  //     navigate('/');
  //   } catch (error) {
  //     console.error('Error logging in', error);
  //   }
  // };

  // Simuleer een succesvolle login
  if (username === "username" && password === "password") {
    const mockResponse = { token: "mock_token" };
    localStorage.setItem('token', mockResponse.token);
    navigate('/');
} else {
    console.error('Invalid credentials');
    alert("Ongeldige inloggegevens");
}
};

  return (
    <form onSubmit={handleLogin}>
        <div>
            <label>Username:</label>
            <input type="username" value={username} onChange={(e) => setUsername(e.target.value)} required />
        </div>
        <div>
            <label>Wachtwoord:</label>
            <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </div>
        <button type="submit">Inloggen</button>
    </form>
);
};

export default Login;
  
    