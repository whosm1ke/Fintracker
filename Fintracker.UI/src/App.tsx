import './index.css'
import useUserStore from "./stores/userStore.ts";



function App() {
const user = useUserStore(x => x.user);
console.log(user);
    return (
        <div>
            
        </div>
    );
}

export default App;
