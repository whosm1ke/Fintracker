import {createBrowserRouter} from "react-router-dom";
import ErrorPage from "./pages/ErrorPage.tsx";
import Layout from "./pages/Layout.tsx";
import RegisterForm from "./components/forms_buttons/RegisterForm.tsx";
import LoginForm from "./components/forms_buttons/LoginForm.tsx";

const router = createBrowserRouter([
    {
        id: 'root',
        path: '/',
        element: <Layout/>,
        errorElement: <ErrorPage/>,
        children: [
          
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
    }
   
])

export default router; 