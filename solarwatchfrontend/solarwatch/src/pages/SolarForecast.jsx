import React from 'react';
import {useEffect, useState} from "react";
import {Link, useLocation, useNavigate} from "react-router-dom";
import {useCookies} from "react-cookie";
import LogoutButton from "../components/LogoutButton.jsx";

function SolarForecast({setUserLoginCookies}) {
    const navigate = useNavigate();
    const location = useLocation();
    const [cookies] = useCookies(['user']);
    const [solarData, setSolarData] = useState({});
    const [solarForecast, setSolarForecast] = useState({});

    const getSolarForecast = async () => {
        const response = await fetch("/api/SolarWatch/GetSolarWatch?date="+solarData.Date+"&cityName="+solarData.CityName,{
            method: "GET",
            headers: {"Authorization": "Bearer " + cookies.user,
            'Content-Type': 'application/json'}
        });
        const data = await response.json();
        console.log(data);
        return data;
    }


    useEffect(() => {
        if (!cookies.user) {
            navigate('/login');
        }
    }, [cookies]);

    const handleLogout = async () => {
        setUserLoginCookies(null);
    };

    return (
        <div>
            <LogoutButton handleLogout={handleLogout}/>
            <div className='relative flex flex-col justify-center'>
                <div className='relative flex justify-center top-24'>
                    <div className='relative  p-4 bg-white border border-gray-200 rounded-lg shadow sm:p-6 md:p-8'>
                        <form className='w-80 max-w-screen-lg sm:w-96 m-auto'>
                            <h5 className='text-xl font-medium text-gray-900'>SolarForecast</h5>
                            <br/>
                            <div className='flex flex-col gap-6'>
                                <label className='block text-sm font-medium text-gray-900  -mb-4'>
                                    City Name:
                                </label>
                                <input
                                    onChange={(event) => {
                                        setSolarData({...solarData, CityName: event.target.value});
                                    }}
                                    type='text'
                                    required
                                    placeholder='cityname'
                                    className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white'
                                />
                                <label className='block  text-sm font-medium text-gray-900  -mb-4'>
                                    Date:
                                </label>
                                <input
                                    placeholder='date'
                                    className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white'
                                    onChange={(event) => {
                                        setSolarData({...solarData, Date: event.target.value});
                                    }}
                                    type='date'
                                    required
                                />
                            </div>
                            <button
                                onClick={(event) => {
                                    event.preventDefault();
                                    getSolarForecast(event).then((data) => {setSolarForecast(data);})
                                    }
                                }
                                className='w-full mt-6 bg-blue-500 hover:bg-blue-600 text-white focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center cursor-pointer'
                            >
                                GetSolarForecast
                            </button>
                        </form>
                    </div>
                </div>
                {solarForecast && (
                    <div className='relative flex justify-center top-36'>
                        <div className='relative  p-4 bg-white border border-gray-200 rounded-lg shadow sm:p-6 md:p-8'>
                            <div className='w-80 max-w-screen-lg sm:w-96 m-auto'>
                                <h5 className='text-xl font-medium text-gray-900'>{solarData.CityName}</h5>
                                <br/>
                                <div className='flex flex-col gap-6'>
                                    <label className='block text-sm font-medium text-gray-900  -mb-4'>
                                        Sunrise: {solarForecast.sunrise}
                                    </label>
                                    <label className='block  text-sm font-medium text-gray-900  -mb-4'>
                                        Sunset: {solarForecast.sunset}
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>)}
            </div>
        </div>
    );
}

export default SolarForecast;