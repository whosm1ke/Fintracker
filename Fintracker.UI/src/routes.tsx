﻿import {createBrowserRouter, Navigate} from "react-router-dom";
import ErrorPage from "./pages/ErrorPage.tsx";
import RegisterForm from "./components/forms_buttons/RegisterForm.tsx";
import LoginForm from "./components/forms_buttons/LoginForm.tsx";
import HomePage from "./pages/HomePage.tsx";
import Layout from "./pages/Layout.tsx";
import AboutPage from "./pages/AboutPage.tsx";
import BankPage from "./pages/BankPage.tsx";
import FintrackerPage from "./pages/FintrackerPage.tsx";
import DashboardLayout from "./pages/DashboardLayout.tsx";
import WalletLayout from "./pages/WalletLayout.tsx";
import WalletOverviewPage from "./pages/WalletOverviewPage.tsx";
import WalletSettingsPage from "./pages/WalletSettingsPage.tsx";
import WalletSettingsCategoriesPage from "./pages/WalletSettingsCategoriesPage.tsx";
import WalletTransactionsPage from "./pages/WalletTransactionsPage.tsx";
import BudgetDetailsPage from "./pages/BudgetDetailsPage.tsx";
import BudgetsPage from "./pages/BudgetsPage.tsx";

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