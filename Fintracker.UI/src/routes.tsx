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
import WalletLayout from "./pages/Layout/WalletLayout.tsx";
import WalletTransactionsPage from "./pages/wallet/WalletTransactionsPage.tsx";
import WalletOverviewPage from "./pages/wallet/WalletOverviewPage.tsx";
import WalletGeneralSettingsPage from "./pages/wallet/WalletGeneralSettingsPage.tsx";
import WalletCategoriesSettingsPage from "./pages/wallet/WalletCategoriesSettingsPage.tsx";
import RegisterForm from "./components/auth/RegisterForm.tsx";
import LoginForm from "./components/auth/LoginForm.tsx";
import WalletSettingsLayoutPage from "./pages/Layout/WalletSettingsLayoutPage.tsx";
import InviteAccept from "./components/auth/InviteAccept.tsx";

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
            }
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
                id: 'walletSettings',
                path: ':walletId/settings',
                element: <WalletSettingsLayoutPage/>,
                children: [
                    {
                        index: true,
                        id: 'walletSettingsGeneral',
                        element: <WalletGeneralSettingsPage/>
                    },
                    {
                        id: 'walletSettingsCategories',
                        path: "categories",
                        element: <WalletCategoriesSettingsPage/>
                    }
                ]
            },
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
    },
    {
        id:'confirm-invite',
        path: 'confirm-invite',
        element: <InviteAccept/>
    }
])


export default router; 