import { NavLink } from 'react-router-dom';
import Account from '../../public/photos/account.png';

const UserNotLoggedIn = () => {
  return (
    <div className="flex items-center justify-center h-screen bg-gray-100">
      <div className="text-center bg-white p-12 rounded-lg shadow-lg max-w-lg mx-auto">
        <img
          src={Account}
          alt="User not logged in"
          className="w-24 h-24 mx-auto mb-6"
        />
        <h1 className="text-4xl font-bold text-gray-800 mb-4">
          Oops! You're Not Logged In
        </h1>
        <p className="text-lg text-gray-600 mb-6">
          It looks like you need to log in to access this page. Please log in or
          sign up to continue.
        </p>

        <NavLink
          to="/auth/signin"
          className="inline-block bg-primary text-white px-8 py-4 rounded-lg shadow hover:bg-white hover:text-black transition-colors duration-300"
        >
          Log In
        </NavLink>

        <p className="mt-4 text-gray-500">
          Don't have an account?{' '}
          <NavLink to="/auth/signup" className="text-blue-500 hover:underline">
            Sign Up
          </NavLink>
        </p>
      </div>
    </div>
  );
};

export default UserNotLoggedIn;
