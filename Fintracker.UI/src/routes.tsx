import {createBrowserRouter} from "react-router-dom";
import ErrorPage from "./pages/ErrorPage.tsx";
import RegisterForm from "./components/forms_buttons/RegisterForm.tsx";
import LoginForm from "./components/forms_buttons/LoginForm.tsx";
import HomePage from "./pages/HomePage.tsx";

const router = createBrowserRouter([
    {
        id: 'root',
        path: '/',
        element: <HomePage/>,
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
    },
   
])

export default router; 