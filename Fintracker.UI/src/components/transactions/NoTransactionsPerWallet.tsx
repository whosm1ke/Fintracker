export default function NoTransactionsPerWallet() {

    return (
        <div className="w-full flex flex-col justify-center items-center p-12 gap-5">
            <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink" width="194" height="148"
                 viewBox="0 0 194 148">
                <defs>
                    <filter id="empty-wallet-a" width="100%" height="100%" x="0%" y="0%"
                            filterUnits="objectBoundingBox">
                        <feGaussianBlur in="SourceGraphic"></feGaussianBlur>
                    </filter>
                    <path id="empty-wallet-b"
                          d="M15 10c0-2.21 1.796-4.122 3.99-4.271L97.01.426C99.214.276 101 1.952 101 4.163v37.829A4.008 4.008 0 0 1 96.996 46H19.004A4.006 4.006 0 0 1 15 42V10z"></path>
                    <path id="empty-wallet-c"
                          d="M92.628 40.061c.534-.818 1.362-1.454 2.288-1.666l10.168-2.321c1.61-.368 2.916.676 2.916 2.331v9.003c0 1.104-1.31 2.299-2.916 2.666l-3.16.721a6.984 6.984 0 0 1-5.918 3.266h-9.012c-3.862 0-6.994-3.142-6.994-7 0-3.866 3.126-7 6.994-7h5.634z"></path>
                    <path id="empty-wallet-d"
                          d="M15 24.7V9c0 1.55 1.778 2.954 3.983 3.14l82.034 6.882c2.2.184 3.983 2.132 3.983 4.328v61.207c0 2.206-1.778 3.845-3.983 3.66l-82.034-6.883C16.783 81.15 15 79.206 15 76.996V24.701z"></path>
                    <filter id="empty-wallet-e" width="106.7%" height="107.6%" x="-3.3%" y="-3.8%"
                            filterUnits="objectBoundingBox">
                        <feOffset dx="-6" dy="-4" in="SourceAlpha" result="shadowOffsetInner1"></feOffset>
                        <feComposite in="shadowOffsetInner1" in2="SourceAlpha" k2="-1" k3="1" operator="arithmetic"
                                     result="shadowInnerInner1"></feComposite>
                        <feColorMatrix in="shadowInnerInner1"
                                       values="0 0 0 0 0.898039216 0 0 0 0 0.921568627 0 0 0 0 0.933333333 0 0 0 0.5 0"></feColorMatrix>
                    </filter>
                    <path id="empty-wallet-f"
                          d="M94.763 42.474c.069.004.139.011.209.02l10.056 1.354c1.641.221 2.972-.938 2.972-1.39v12.783c0 1.661-1.337 2.827-2.972 2.607l-8.709-1.172a6.128 6.128 0 0 1-.258-.029L86.94 55.43A7.996 7.996 0 0 1 80 47.504a6.128 6.128 0 0 1 6.939-6.074l7.824 1.044z"></path>
                    <filter id="empty-wallet-g" width="110.7%" height="118.2%" x="-5.4%" y="-9.1%"
                            filterUnits="objectBoundingBox">
                        <feOffset dx="-3" dy="-3" in="SourceAlpha" result="shadowOffsetInner1"></feOffset>
                        <feComposite in="shadowOffsetInner1" in2="SourceAlpha" k2="-1" k3="1" operator="arithmetic"
                                     result="shadowInnerInner1"></feComposite>
                        <feColorMatrix in="shadowInnerInner1"
                                       values="0 0 0 0 0.898039216 0 0 0 0 0.921568627 0 0 0 0 0.933333333 0 0 0 0.5 0"></feColorMatrix>
                    </filter>
                    <path id="empty-wallet-h"
                          d="M40.785 17.85c-3.19.089-6.155-.375-8.894-1.394-4.109-1.527-4.643-3.474-7.846-3.282-3.202.193-15.002 2.712-17.934 6.834-1.954 2.748.4 3.124 7.061 1.128-1.613 1.085-2.197 1.984-1.75 2.697.445.712 2.608.356 6.486-1.069-1.734 1.39-2.49 2.362-2.268 2.915.222.553 2.714.277 7.475-.83-.96 1.04-1.2 1.689-.72 1.946.72.385 6.222-.93 8.683-1.946 2.461-1.016 5.683-3.121 8.678-3.084 1.998.024 2.34-1.28 1.029-3.916z"></path>
                    <filter id="empty-wallet-i" width="108.3%" height="121.9%" x="-4.2%" y="-11%"
                            filterUnits="objectBoundingBox">
                        <feOffset dx="-1" dy="-2" in="SourceAlpha" result="shadowOffsetInner1"></feOffset>
                        <feComposite in="shadowOffsetInner1" in2="SourceAlpha" k2="-1" k3="1" operator="arithmetic"
                                     result="shadowInnerInner1"></feComposite>
                        <feColorMatrix in="shadowInnerInner1"
                                       values="0 0 0 0 0.898039216 0 0 0 0 0.921568627 0 0 0 0 0.933333333 0 0 0 0.5 0"></feColorMatrix>
                    </filter>
                    <path id="empty-wallet-j"
                          d="M45.543 71.571c-3.19.09-6.155-.375-8.894-1.393-4.108-1.527-4.643-3.474-7.845-3.282-3.203.192-15.002 2.712-17.934 6.834-1.955 2.748.399 3.124 7.06 1.128-1.613 1.085-2.196 1.984-1.75 2.697.446.712 2.608.356 6.487-1.069-1.734 1.39-2.49 2.362-2.269 2.915.222.553 2.714.277 7.476-.83-.96 1.04-1.2 1.688-.72 1.945.72.386 6.221-.93 8.682-1.945 2.461-1.016 5.683-3.122 8.679-3.084 1.997.024 2.34-1.28 1.028-3.916z"></path>
                    <filter id="empty-wallet-k" width="116.6%" height="143.8%" x="-8.3%" y="-21.9%"
                            filterUnits="objectBoundingBox">
                        <feOffset dx="5" in="SourceAlpha" result="shadowOffsetInner1"></feOffset>
                        <feComposite in="shadowOffsetInner1" in2="SourceAlpha" k2="-1" k3="1" operator="arithmetic"
                                     result="shadowInnerInner1"></feComposite>
                        <feColorMatrix in="shadowInnerInner1"
                                       values="0 0 0 0 0.898039216 0 0 0 0 0.921568627 0 0 0 0 0.933333333 0 0 0 0.5 0"></feColorMatrix>
                    </filter>
                    <path id="empty-wallet-l"
                          d="M3.524 33.927s16 1.126 24.364.3c8.364-.826 33.467-7.665 33.467-7.665l.12 28.432s-27.461 3-40.958 3.583C7.021 59.16 2.9 57.492 2.9 57.492l.624-23.565z"></path>
                </defs>
                <g fill="none" fillRule="evenodd">
                    <g transform="translate(0 54)">
                        <ellipse cx="60" cy="83" fill="#E5EBEE" fillOpacity=".5" filter="url(#empty-wallet-a)" rx="60"
                                 ry="11"></ellipse>
                        <use fill="#E5EBEE" xlinkHref="#empty-wallet-b"></use>
                        <path stroke="#BACDD7"
                              d="M15.5 10v32c0 1.93 1.57 3.5 3.504 3.5h77.992a3.508 3.508 0 0 0 3.504-3.508V4.163c0-1.923-1.542-3.368-3.456-3.238l-78.02 5.303C17.089 6.359 15.5 8.058 15.5 10z"></path>
                        <g transform="matrix(1 0 0 -1 0 90.061)">
                            <use fill="#E5EBEE" xlinkHref="#empty-wallet-c"></use>
                            <path stroke="#BACDD7"
                                  d="M93.046 40.335l-.148.226h-5.904a6.494 6.494 0 0 0-6.494 6.5c0 3.585 2.91 6.5 6.494 6.5h9.012a6.483 6.483 0 0 0 5.495-3.033l.11-.174 3.362-.768c1.404-.32 2.527-1.346 2.527-2.178v-9.003c0-1.335-1.007-2.14-2.305-1.844l-10.168 2.322c-.77.176-1.499.714-1.98 1.452z"></path>
                        </g>
                        <use fill="#FFF" xlinkHref="#empty-wallet-d"></use>
                        <use fill="#000" filter="url(#empty-wallet-e)" xlinkHref="#empty-wallet-d"></use>
                        <path stroke="#BACDD7"
                              d="M15.5 11.163v65.833c0 1.951 1.585 3.677 3.525 3.84l82.033 6.882c1.913.16 3.442-1.246 3.442-3.161V23.35c0-1.94-1.588-3.667-3.525-3.83l-82.033-6.882c-1.382-.116-2.625-.675-3.442-1.475z"></path>
                        <g>
                            <use fill="#FFF" xlinkHref="#empty-wallet-f"></use>
                            <use fill="#000" filter="url(#empty-wallet-g)" xlinkHref="#empty-wallet-f"></use>
                            <path stroke="#BACDD7"
                                  d="M107.5 43.768c-.694.428-1.65.695-2.539.576L94.905 42.99a2.503 2.503 0 0 0-.174-.017l-.035-.004-7.824-1.044a5.628 5.628 0 0 0-6.372 5.579 7.496 7.496 0 0 0 6.505 7.43l9.123 1.217c.078.01.157.02.258.03l8.709 1.172c1.337.18 2.405-.756 2.405-2.112V43.768z"></path>
                        </g>
                        <circle cx="87" cy="48.3" r="4" fill="#BACDD7"></circle>
                    </g>
                    <path stroke="#BACDD7" strokeDasharray="4,3"
                          d="M105 88c19.548-12.065 21.149-20.945 21.542-24.893.393-3.948-3.308-8.122-8.098-6.181-4.79 1.94-4.685 5.52-1.953 8.666 2.733 3.146 14.364 5.496 21.202-1.262 4.558-4.505 7.525-7.89 8.903-10.154"></path>
                    <g transform="rotate(18 74.168 394.156)">
                        <use fill="#FFF" xlinkHref="#empty-wallet-h"></use>
                        <use fill="#000" filter="url(#empty-wallet-i)" xlinkHref="#empty-wallet-h"></use>
                        <use stroke="#BACDD7" xlinkHref="#empty-wallet-h"></use>
                    </g>
                    <g transform="scale(-1 1) rotate(-57 -93.456 260.207)">
                        <use fill="#FFF" xlinkHref="#empty-wallet-j"></use>
                        <use fill="#000" filter="url(#empty-wallet-k)" xlinkHref="#empty-wallet-j"></use>
                        <use stroke="#BACDD7" xlinkHref="#empty-wallet-j"></use>
                    </g>
                    <g transform="rotate(-46 85.683 -117.768)">
                        <use fill="#FFF" xlinkHref="#empty-wallet-l"></use>
                        <path stroke="#BACDD7"
                              d="M4.01 34.46l-.6 22.673c.067.02.144.041.23.065.513.137 1.17.275 1.983.404 3.39.54 8.266.761 14.873.475 4.157-.18 9.89-.598 16.8-1.204a985.45 985.45 0 0 0 5.898-.536 1341.917 1341.917 0 0 0 17.78-1.791l-.117-27.331-.607.163a654.093 654.093 0 0 1-13.796 3.543c-1.042.255-2.061.5-3.055.737-7.051 1.676-12.42 2.766-15.462 3.066-2.506.248-5.822.337-9.755.296-2.99-.031-6.187-.137-9.417-.292a300.445 300.445 0 0 1-4.755-.267z"></path>
                    </g>
                    <path fill="#E5EBEE"
                          d="M171.015 29.316a7.647 7.647 0 0 0-10.816.185c-2.933 3.039-2.846 7.883.195 10.82a7.647 7.647 0 0 0 10.815-.186 7.654 7.654 0 0 0-.194-10.819m-12.352 28.192c-2.385-2.304-5.59-2.247-7.893.137l-5.765-5.797c2.301-2.384 1.883-5.941-.503-8.245 0 0 9.49-9.447 13.728-15.133 4.31-5.78 11.916-19.267 11.916-19.267 2.385 2.304 6.308 2.234 8.61-.15l7.098 6.87c-2.301 2.385-2.423 6.063-.038 8.366 0 0-8.87 12.244-13.547 17.966-4.374 5.35-13.606 15.253-13.606 15.253z"></path>
                    <path fill="none" d="M-107-36h330v194h-330z"></path>
                </g>
            </svg>
            <span>There is no transactions for now</span>
        </div>
    )
}