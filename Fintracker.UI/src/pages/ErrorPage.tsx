import {isRouteErrorResponse, useRouteError} from "react-router-dom";

export default function ErrorPage() {
    console.log('ERROR')
    const error = useRouteError();
    return (
        <>
            <h1>Oops</h1>
            <div>{
                isRouteErrorResponse(error)
                    ? "This page does not exists"
                    : "Unexpected error occured"
            }</div>
        </>
    )
}
