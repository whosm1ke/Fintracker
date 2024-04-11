import {createBrowserRouter} from "react-router-dom";
import ErrorPage, {Error} from "./pages/ErrorPage.tsx";
import RegisterForm from "./components/forms_buttons/RegisterForm.tsx";
import LoginForm from "./components/forms_buttons/LoginForm.tsx";
import HomePage from "./pages/HomePage.tsx";
import Layout from "./pages/Layout.tsx";
import AboutPage from "./pages/AboutPage.tsx";

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
                id:'about',
                path:'about',
                element: <AboutPage/>,
                errorElement: <Error/>
            }
        ],
        
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
   
])

export default router; 