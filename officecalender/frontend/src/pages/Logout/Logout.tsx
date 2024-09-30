import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Logout = () => {
    const navigate = useNavigate();

    useEffect(() => {
        // Delete token 
        localStorage.removeItem('token');
        // Navigate to login
        navigate('/login');
    }, [navigate]);

    return <div>U bent uitgelogd!</div>;
};

export default Logout;