import {Link} from "react-router-dom";

interface SubTitleProps {
    h1: 'Sign up' | 'Sign in',
    h4: 'Already registered?' | 'Don`t have an account?'
    linkTo: "login" | 'registration',
    linkText: 'Sign up' | 'Sign in'
}
const SubTitle = ({h1, h4, linkTo, linkText} : SubTitleProps) => {
    return (
        <header>
            <h1 className={"text-center text-2xl"}>
                <span className={'font-bold'}>{h1} </span>
                for Fintracker</h1>
            <h4 className={'text-center mt-2'}>{h4} {" "}
                <Link to={`/${linkTo}`} className={'underline text-green-300'}>{linkText}</Link>
            </h4>
        </header>
    )
}

export default SubTitle;