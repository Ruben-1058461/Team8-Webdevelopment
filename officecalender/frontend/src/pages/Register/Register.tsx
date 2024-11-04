import React, { useState } from 'react';
import axios from 'axios';

const Register = () => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleRegister = async () => {
        try {
            const response = await axios.post('http://localhost:5002/api/user/register', {
                firstName,
                lastName,
                email,
                password
            });
            console.log(response.data);
        } catch (error) {
            console.error('Error registering user', error);
        }
    };

    return (
        <div>
            <h2>Register</h2>
            <input type="text" placeholder="First Name" onChange={(e) => setFirstName(e.target.value)} />
            <input type="text" placeholder="Last Name" onChange={(e) => setLastName(e.target.value)} />
            <input type="email" placeholder="Email" onChange={(e) => setEmail(e.target.value)} />
            <input type="password" placeholder="Password" onChange={(e) => setPassword(e.target.value)} />
            <button onClick={handleRegister}>Register</button>
        </div>
    );
};

export default Register;
