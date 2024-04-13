import {GrShieldSecurity} from "react-icons/gr";
import { SiSecurityscorecard } from "react-icons/si";
import {Bs1SquareFill, Bs2SquareFill, Bs3SquareFill, Bs4SquareFill} from "react-icons/bs";

export const textFiller = [
    {
        title: 'Why should you use Fintracker?',
        content: "Fintracker is a tool you don\'t want to miss. It helps you stay in control of your expenses, income and savings, making it easy to track where your money is going and plan your budget. With automatic synchronisation with your bank accounts and credit cards, " +
            "Fintracker lets you track all your transactions in real time. Fintracker also analyses your spending and provides personalised recommendations to help you better manage your finances. Your data is safe with Fintracker because we use advanced encryption technology to keep your information private." +
            "You can use Fintracker on any device, allowing you to manage your finances wherever you are. Fintracker is your key to a successful financial future. With it, you can lead a more informed and responsible financial life. Join us today!",
        imagePath: "../../src/assets/logo.png"
    },
    {
        title: 'Why manage finances?',
        content: "Managing finances is a crucial aspect of life that affects everyone, regardless of income or social status. It involves budgeting, which allows you to understand where your money is going and where you can save. It also enables you to plan for the future, whether that’s buying a house, funding your children’s education, or preparing for retirement." +
            "Effective financial management helps prevent debt by controlling spending and ensuring you live within your means. It also aids in achieving financial independence, allowing you to live comfortably without the constant need to earn." +
            "Moreover, managing finances can help prevent stress, as financial problems are one of the leading causes of stress. It provides financial stability, which can alleviate this stress." +
            "Lastly, financial management helps you understand when and how to invest money to increase your wealth. Therefore, financial management is an essential tool that helps you manage your money effectively and achieve your financial goals. Without it, you risk losing control of your finances and facing financial difficulties.",
        imagePath: "../../src/assets/wallet.svg"
    }
]

export const advantages = [
    {
        title: "Budgeting",
        description: "Managing finances allows you to create a budget, giving you a clear understanding of your income and expenses. This can help you identify areas where you can save money."
    },
    {
        title: "Future Planning",
        description: "Financial management enables you to plan for the future, whether it's saving for a house, your children's education, or retirement."
    },
    {
        title: "Debt Avoidance",
        description: "By controlling your spending and living within your means, financial management can help you avoid falling into debt."
    },
    {
        title: "Financial Independence",
        description: "Effective financial management can lead to financial independence, allowing you to live comfortably without the constant need to earn more money."
    },
    {
        title: "Stress Reduction",
        description: "Financial problems are a leading cause of stress. By providing financial stability, managing your finances can help alleviate this stress."
    },
];

export const about = [
    {
        title: 'Your financial assistant',
        content: 'We are a new startup that aims to help people manage their finances effectively. Our mission is to provide you with the tools to monitor your expenses and plan budgets so you can focus on what really matters.'
    },
    {
        title: 'Our story',
        content: 'Although our company is new to the market, we are working to become your trusted partner in financial planning. Our story begins with one student who dreamed of making financial management easier and more accessible to everyone.'
    },
    {
        title: 'Meet the developer',
        content: 'Our product was developed by one person - a 21-year-old KPI student. He dedicates his time and efforts to making financial management easier and more accessible for everyone.'
    },
    {
        title: 'Our values',
        content: 'We strive for security, flexibility and customer focus in everything we do. We believe these values are the key to providing you with the best possible service.'
    },
    {
        title: 'Our services',
        content: 'We specialise in financial management and bank statement monitoring. Our tools will help you better understand your expenses and plan your budget.'
    },
    {
        title: 'Contact us',
        content: 'If you have any questions or would like to know more, please email us at kazimirka1234@gmail.com.'
    },
    {
        title: 'Join us',
        content: 'Start your journey to better financial management today. Join us and find out how we can help you better control your spending and plan your budget.'
    }
]

export const security = [
    {
        title: 'Secured infrastructure',
        content: 'As security is our highest priority, all communication is encrypted. Whenever your data travels between our server and your phone, or we download your transactions from your bank, it’s done only through encrypted channels so nobody can see the data on the way. Once your data gets to our server, it’s stored and also encrypted so only authorized users can access it. We only use reliable providers to host our servers. Right now, we use the Google Cloud Platform which meets the strictest security compliance policies.',
        icon: <GrShieldSecurity size={'3rem'} color={'yellow'}/>
    },
    {
        title: 'Secured bank connection',
        content: 'We work with all financial account information in a read-only mode to be able to check your account balance and download lists of transactions. However, we are not able to initiate payments or manipulate your account in any other way. The only minimum required amount of sensitive data (login credentials) is stored, only if you opt in to do so. For some banks, you have to give us your password, to be able to access the bank data. In this case, the password is stored only in safe, encrypted storage. For directly connected banks we don’t even see your password. You login directly in your bank and we receive just an authorization token with limited life-time.',
        icon: <SiSecurityscorecard size={'3rem'} color={'green'}/>
    }
]

export const bankConnect = [
    {
        title: 'Visit monobank',
        content: 'Go to official monobank api web site (https://api.monobank.ua/index.html)',
        icon: <Bs1SquareFill  size={'2.5rem'} color={'gray'}/>
    },
    {
        title: 'Scan',
        content: 'Scan a QR-code from their site',
        icon: <Bs2SquareFill  size={'2.5rem'} color={'gray'}/>
    },
    {
        title: 'Copy & Provide',
        content: 'Copy token and paste it in the wallet while creating it',
        icon: <Bs3SquareFill  size={'2.5rem'} color={'gray'}/>
    },
    {
        title: 'Profit',
        content: 'You are good to go. Now you can check your bank transactions from our app',
        icon: <Bs4SquareFill  size={'2.5rem'} color={'gray'}/>
    },
    
]

