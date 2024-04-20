import {createBrowserRouter, Navigate} from "react-router-dom";
import Layout from "./pages/Layout/Layout";
import ErrorPage from "./pages/ErrorPage";
import HomePage from "./pages/start/HomePage.tsx";
import AboutPage from "./pages/start/AboutPage.tsx";
import BankPage from "./pages/start/BankPage.tsx";
import DashboardLayout from "./pages/Layout/DashboardLayout.tsx";
import FintrackerPage from "./pages/start/FintrackerPage.tsx";
import BudgetsPage from "./pages/budget/BudgetsPage.tsx";
import BudgetDetailsPage from "./pages/budget/BudgetDetailsPage.tsx";
import WalletLayout from "./pages/wallet/WalletLayout.tsx";
import WalletTransactionsPage from "./pages/wallet/WalletTransactionsPage.tsx";
import WalletOverviewPage from "./pages/wallet/WalletOverviewPage.tsx";
import WalletSettingsPage from "./pages/wallet/WalletSettingsPage.tsx";
import WalletSettingsCategoriesPage from "./pages/wallet/WalletSettingsCategoriesPage.tsx";
import RegisterForm from "./components/forms_buttons/RegisterForm.tsx";
import LoginForm from "./components/forms_buttons/LoginForm.tsx";

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
                path: 'bank',
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
            },
            {
                id: 'budgets',
                path: 'budgets',
                element: <BudgetsPage/>
            },
            {
                id: 'budgetDetails',
                path: 'budgets/:budgetId',
                element: <BudgetDetailsPage/>
            }
        ]
    },
    {
        id: 'wallet',
        path: "wallet",
        element: <WalletLayout/>,
        children: [
            {
                index: true,
                element: <Navigate to={"/dashboard"} replace={true}/>
            },
            {
                id: 'walletTransactions',
                path: ":walletId/trans",
                element: <WalletTransactionsPage/>
            },
            {
                id: 'walletOverview',
                path: ":walletId/overview",
                element: <WalletOverviewPage/>
            },
            {
                id: 'walletBudgets',
                path: ':walletId/budgets',
                element: <BudgetsPage/>
            },
            {
                id: 'walletBudgetDetails',
                path: ':walletId/budgets/:budgetId',
                element: <BudgetDetailsPage/>
            },
            {
                id: 'walletSettingsGeneral',
                path: ":walletId/settings/general",
                element: <WalletSettingsPage/>
            },
            {
                id: 'walletSettingsCategories',
                path: ":walletId/settings/categories",
                element: <WalletSettingsCategoriesPage/>
            }
        ]
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