import ReactDOM from 'react-dom/client'
import './index.css'
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {RouterProvider} from "react-router-dom";
import {ReactQueryDevtools} from "@tanstack/react-query-devtools";
import React from 'react';
import routes from "./routes.tsx";

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            refetchOnWindowFocus: false,
        }
    }
});

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
                <RouterProvider router={routes} />
            <ReactQueryDevtools/>
        </QueryClientProvider>
    </React.StrictMode>
)
