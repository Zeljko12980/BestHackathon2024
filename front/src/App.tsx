import { useEffect, useState } from 'react';
import { Route, Routes, useLocation } from 'react-router-dom';

import Loader from './common/Loader';
import PageTitle from './components/PageTitle';
import SignIn from './pages/Authentication/SignIn';
import Chart from './pages/Chart';
import DefaultLayout from './layout/DefaultLayout';
import HomePage from './pages/HomePage/HomePage';
import ClassDetails from './pages/ClassDetails';
import Statistika from './pages/Statistics';
import GamePage from './pages/GamePage';
import TableStudents from './pages/TableStudents';

function App() {
  const [loading, setLoading] = useState<boolean>(true);
  const { pathname } = useLocation();

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);

  useEffect(() => {
    setTimeout(() => setLoading(false), 1000);
  }, []);

  return loading ? (
    <Loader />
  ) : (
    <DefaultLayout>
      <Routes>
        <Route
          index
          element={
            <>
              <PageTitle title="eCommerce Dashboard | TailAdmin - Tailwind CSS Admin Dashboard Template" />
              <HomePage />
            </>
          }
        />

        <Route
          path="/chart"
          element={
            <>
              <PageTitle title="Statistika" />
              <Chart />
            </>
          }
        />

        <Route
          path="/auth/signin"
          element={
            <>
              <PageTitle title="Signin | TailAdmin - Tailwind CSS Admin Dashboard Template" />
              <SignIn />
            </>
          }
        />

        <Route
          path="/class/:classId"
          element={
            <>
              <PageTitle title="Detalji časa" />
              <ClassDetails />
            </>
          }
        />
        <Route
          path="/game"
          element={
            <>
              <PageTitle title="Detalji časa" />
              <GamePage />
            </>
          }
        />
        <Route
          path="//statistics/:className"
          element={
            <>
              <PageTitle title="Statistika razreda" />
              <Statistika />
            </>
          }
        />
        <Route
          path="table"
          element={
            <>
              <PageTitle title="Statistika razreda" />
              <TableStudents />
            </>
          }
        />
      </Routes>
    </DefaultLayout>
  );
}

export default App;
