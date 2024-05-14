interface TransactionItemSelectorProps {
    item: TransactionSelectorMap
}

export interface TransactionSelectorMap {
    id: string;
    value: number | undefined
}

export default function TransactionsPerDateItem ({item}: TransactionItemSelectorProps) {
    return (
        <div className={'flex items-center justify-between'}>
            <p className={'font-bold text-md'}>{item.value || "All"}</p>
            {item.value && <p className={'italic'}>transactions</p>}
        </div>
    )
}