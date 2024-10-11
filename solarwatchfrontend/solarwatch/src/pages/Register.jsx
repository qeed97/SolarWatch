import React from 'react';
import {useEffect, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import {toast} from 'react-hot-toast';

function Register(props) {
    const [newUserData, setNewUserData] = useState({});
    const showSuccessToast = (data) => toast.success(data.message);
    const showErrorToast = (data) => toast.error(data.message);

    const handleSignup = async () => {
        const response = await fetch('/api/Auth/Register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newUserData)
        });
        const data = await response.json();
        logging(data);
        return data;
    };

    function logging(data) {
        console.log(data);
    }

    return (
        <div className='relative flex justify-center top-48'>
            <div className='relative  p-4 bg-white border border-gray-200 rounded-lg shadow sm:p-6 md:p-8'>
                <form className='w-80 max-w-screen-lg sm:w-96 m-auto'>
                    <h5 className='text-xl font-medium text-gray-900'>Registration</h5>
                    <br/>
                    <div className='flex flex-col gap-6'>
                        <label className='block text-sm font-medium text-gray-900  -mb-4'>
                            Your Username:
                        </label>
                        <input
                            onChange={(event) => {
                                setNewUserData({...newUserData, UserName: event.target.value});
                            }}
                            type='text'
                            required
                            placeholder='username'
                            className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white'
                        />
                        <label className='block  text-sm font-medium text-gray-900  -mb-4'>
                            Your Email:
                        </label>
                        <input
                            placeholder='name@mail.com'
                            className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white'
                            onChange={(event) => {
                                setNewUserData({...newUserData, Email: event.target.value});
                            }}
                            type='email'
                            required
                        />
                        <label className='block text-sm font-medium text-gray-900  -mb-4'>
                            Password:
                        </label>
                        <input
                            type='password'
                            placeholder='********'
                            className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white'
                            onChange={(event) => {
                                setNewUserData({
                                    ...newUserData,
                                    Password: event.target.value,
                                });
                            }}
                            required
                        />
                    </div>
                    <button
                        onClick={(event) => {
                            event.preventDefault();
                                handleSignup(event).then((data) => {
                                    if (data.message) {
                                        showErrorToast(data.message);
                                    }
                                })
                            }
                        }
                        className='w-full mt-6 bg-blue-500 hover:bg-blue-600 text-white focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center cursor-pointer'

                    >
                        Sign up
                    </button>
                </form>
                <div className='mt-2'>
          <span className='text-center text-slate-500 font-normal'>
            {' '}
              Already have an account?{' '}
          </span>

                </div>
            </div>
        </div>
    );
}

export default Register;