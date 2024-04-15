import {createBrowserRouter} from "react-router-dom";
import ErrorPage from "./pages/ErrorPage.tsx";
import RegisterForm from "./components/forms_buttons/RegisterForm.tsx";
import LoginForm from "./components/forms_buttons/LoginForm.tsx";
import HomePage from "./pages/HomePage.tsx";
import Layout from "./pages/Layout.tsx";
import AboutPage from "./pages/AboutPage.tsx";
import Motion from "./pages/Motion.tsx";
import BankPage from "./pages/BankPage.tsx";
import FintrackerPage from "./pages/FintrackerPage.tsx";
import DashboardLayout from "./pages/DashboardLayout.tsx";

const router = createBrowserRouter([
    {
        id: 'root',
        path: '/',
        element: <Layout/>,
        errorElement: <ErrorPage/>,
        children: [
            {
                id: 'home',
                index: true,
                element: <HomePage/>
            },
            {
                id: 'about',
                path: 'about',
                element: <AboutPage/>,
                errorElement: <div>DASDASD</div>
            },
            {
                id: 'bank',
                path: '/bank',
                element: <BankPage/>
            },
        ],

    },
    {
        id: 'dashboard',
        path: 'dashboard',
        element: <DashboardLayout/>,
        children: [
            {
                index: true,
                element: <FintrackerPage/>
            }
        ]
    },
    {
        id: 'budgets',
        path: 'budgets',
        element: <FintrackerPage/>
    },
    {
        id: 'registration',
        path: 'registration',
        element: <RegisterForm/>
    },
    {
        id: 'login',
        path: 'login',
        element: <LoginForm/>
    },

    {
        id: 'motion',
        path: 'mo',
        element: <Motion/>
    },

])


export default router; 