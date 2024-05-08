import {useLocation} from "react-router-dom";
import Title from "./Title.tsx";
import {useState} from "react";
import useConfirmResetEmail from "../../hooks/auth/useConfirmResetEmail.ts";

export default function ResetEmail() {
    const loc = useLocation();
    const urlQueryParams = new URLSearchParams(loc.search);
    const token = urlQueryParams.get('token');
    const userId = urlQueryParams.get('userId');
    const newEmail = urlQueryParams.get('newEmail');
    const emailResetMutation = useConfirmResetEmail();
    const [isError, setIsError] = useState(false);
    const [canBeClosed, setCanBeClosed] = useState(false)

    const handleInviteAccept = async () => {
        const mutationResponse = await emailResetMutation.mutateAsync({
            token: token!,
            newEmail: newEmail!,
            userId: userId!
        });
        if (!mutationResponse.success) {
            setIsError(true);
        } else {
            setCanBeClosed(true)
        }
    }
    return (
        <section
            className="flex flex-col min-h-screen bg-gray-50 p-4">
            <Title/>
            {!canBeClosed && <div className="max-w-md w-2/3 mx-auto my-auto order-1 sm:order-2">
                {isError && <span className={'text-red-400 italic'}>Something went wrong.</span>}
                <div>
                    <button type="button"
                            className="submit-register-button"
                            onClick={handleInviteAccept}>
                        Accept Invitation
                    </button>
                </div>
            </div>}
            {canBeClosed &&
                <div className="max-w-md w-2/3 mx-auto my-auto order-1 sm:order-2">
                    <div className={'text-green-400 text-xl text-center'}>
                        This page can be closed now
                    </div>
                </div>
            }
        </section>
    )
}