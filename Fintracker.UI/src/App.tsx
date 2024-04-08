import './index.css'
import { useEffect } from 'react';
import ApiClient from "./services/ApiClient.ts";

async function fetchData() {
    const apiClient = new ApiClient<Currency, Currency>('currency');
    const response = await apiClient.getById("8");
    console.log('response: ', response);
}

function App() {
    useEffect(() => {
        fetchData();
    }, []);

    return (
        <div className={"text-6xl font-bold"}>Hi there!</div>
    );
}

export default App;
