// @ts-ignore
import logo from "../../../public/logo.png"
const Title = () => {
    return (
        <div className={"flex items-center justify-center mt-4 sm:mt-0 order-2 sm:order-1"}>
            <img src={logo} alt="logo" className={"w-20 h-20 mr-4"}/>
            <h1 className={"font-bold text-5xl"}>Fintracker</h1>
        </div>
    )
}

export default Title;