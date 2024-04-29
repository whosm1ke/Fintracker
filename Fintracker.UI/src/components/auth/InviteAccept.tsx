import {useLocation, useNavigate} from "react-router-dom";
import Title from "./Title.tsx";
import useAcceptInvite from "../../hooks/auth/useAcceptInvite.ts";
import {useState} from "react";

export default function InviteAccept() {

    const loc = useLocation();
    const urlQueryParams = new URLSearchParams(loc.search);
    const walletId = urlQueryParams.get('walletId');
    const userId = urlQueryParams.get('userId');
    const acceptInviteMutation = useAcceptInvite();
    const navigate = useNavigate();
    const [isError, setIsError] = useState(false);

    const handleInviteAccept = async () => {
        const mutationResponse = await acceptInviteMutation.mutateAsync({
            walletId: walletId!,
            userId: userId!
        });
        if (!mutationResponse.success) {
            setIsError(true);
        } else {
            navigate("../login", {state: mutationResponse.message, replace: true})
        }
    }

    return (
        <section
            className="flex flex-col min-h-screen bg-gray-50 p-4">
            <Title/>
            <div className="max-w-md w-2/3 mx-auto my-auto order-1 sm:order-2">
                {isError && <span className={'text-red-400 text-sm italic'}>Something went wrong.</span>}
                <div>
                    <button type="button"
                            className="submit-register-button"
                            onClick={handleInviteAccept}>
                        Accept Invitation
                    </button>
                </div>
            </div>
        </section>
    )
}