import React from 'react';
import {useEffect, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import {toast} from 'react-hot-toast';
import {useCookies} from "react-cookie";

function Login({setUserLoginCookies}) {
    const navigate = useNavigate();
    const [userDetails, setUserDetails] = useState({
        Email: null,
        Password: null,
    });
    const [cookies] = useCookies(['user']);

    const showErrorToast = (message) => toast.error(message);
    const showSuccessToast = (message) => toast.success(message);

    useEffect(() => {
        if (cookies.user){
            navigate('/main');
        }
    }, [cookies]);

    const handleLogin = async () => {
        const response = await fetch('/api/Auth/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userDetails)
        });
        const data = await response.json();
        console.log(data);
        return data;
    }

    return (
        <div className='relative flex justify-center top-48'>
            <div
                className='relative w-full max-w-sm p-4 bg-white border border-gray-200 rounded-lg shadow sm:p-6 md:p-8 text-black'>
                <form className='space-y-6' action='#'>
                    <h5 className='text-xl font-medium text-gray-900'>
                        Log in to see solar data in different cities!
                    </h5>
                    <div>
                        <label className='block mb-2 text-sm font-medium text-gray-900'>
                            Your email
                        </label>
                        <input
                            onChange={(event) => {
                                setUserDetails({...userDetails, Email: event.target.value});
                            }}
                            type='email'
                            className='bg-gray-50 border border-gray-300  text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 text-black'
                            placeholder='name@company.com'
                            required
                        />
                    </div>
                    <div>
                        <label className='block mb-2 text-sm font-medium text-gray-900 '>
                            Your password
                        </label>
                        <input
                            onChange={(event) => {
                                setUserDetails({
                                    ...userDetails,
                                    Password: event.target.value,
                                });
                            }}
                            type='password'
                            placeholder='••••••••'
                            className='bg-gray-50 border border-gray-300 text-black text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 '
                            required
                        />
                    </div>
                    <button
                        onClick={async (event) => {
                            event.preventDefault();

                            if (
                                userDetails.Password.match(/([a-z?'!0-9])/gi).join('') ===
                                userDetails.Password
                            ) {
                                try {
                                    const data = await handleLogin();
                                    console.log(data);
                                    setUserLoginCookies(data.token);
                                    showSuccessToast('Successfully signed in!');

                                } catch (e) {
                                    console.error(e);
                                    showErrorToast("Some of your details are invalid");
                                }

                            } else {
                                showErrorToast('That email or password is invalid');
                            }
                        }}
                        type='submit'
                        className='w-full text-white bg-blue-500 hover:bg-blue-600 focus:ring-4 focus:outline-nonefont-medium rounded-lg text-sm px-5 py-2.5 text-center'
                    >
                        Login to your account
                    </button>
                </form>

                <div className='text-sm font-medium mt-2 text-gray-500 '>
                    Not registered?{' '}
                    <Link
                        className='text-blue-700 hover:underline dark:text-blue-500'
                        to='/register'
                    >
                        create one!
                    </Link>
                </div>
            </div>
        </div>
    );
}

export default Login;