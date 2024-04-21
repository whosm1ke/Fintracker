import {useEffect} from "react";

const useStopScrolling = (shouldStop: boolean) => {
    useEffect(() => {
        if (shouldStop) {
            document.body.style.overflow = 'hidden';
        } else {
            document.body.style.overflow = 'unset';
        }

        return () => {
            document.body.style.overflow = 'unset';
        };
    }, [shouldStop]);
}

export default useStopScrolling;