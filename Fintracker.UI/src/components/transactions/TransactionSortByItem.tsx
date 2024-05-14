interface TransactionSortByItemProps {
    item: TransactionSortByParam
}

export interface TransactionSortByParam {
    id: string;
    value: string;
    text: string;
    isDescending: boolean;
}

export default function TransactionSortByItem({item}: TransactionSortByItemProps) {
    return (
        <div className={'flex justify-between items-center'}>
            <p>{item.text}</p>
        </div>
    )
}