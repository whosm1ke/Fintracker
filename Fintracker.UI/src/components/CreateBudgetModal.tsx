import {useState} from "react";
import {ActionButton} from "./ActionButton.tsx";

export const CreateBudgetModal = () => {
    const [isOpen, setIsOpen] = useState(false);
    const handleIsOpen = () => setIsOpen(p => !p);
    return (
        <>
            <ActionButton text={"Add new budget"} onModalOpen={handleIsOpen}/>
        </>
    )
}

const Modal = () => {
    return (
        <div></div>
    )
}