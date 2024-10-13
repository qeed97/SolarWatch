import React from 'react';

function LogoutButton({handleLogout}) {
    return (
        <div className="flex w-full justify-between items-center px-4 py-2">
            <div className="flex items-center">
                <button
                    className="mr-2 border-2 border-red-600 h-10 text-red-700 w-20 px-2 rounded text-sm hover:underline"
                    onClick={handleLogout}
                >
                    Logout
                </button>
            </div>
        </div>
    );
}

export default LogoutButton;