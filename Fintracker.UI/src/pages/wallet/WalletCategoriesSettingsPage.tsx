import {SubmitHandler, useForm} from "react-hook-form";
import {Category, nameRegisterOptionsForCategory} from "../../entities/Category.ts";
import {useEffect, useRef, useState} from "react";
import {CategoryType} from "../../entities/CategoryType.ts";
import CategoryItem from "../../components/categories/CategoryItem.tsx";
import icons from "../../data/icons.ts";
import SingleSelectDropDownMenu from "../../components/other/SingleSelectDropDownMenu.tsx";
import colors from "../../data/colors.ts";
import useCreateCategory from "../../hooks/categories/useCreateCategory.ts";
import {handleServerErrorResponse} from "../../helpers/handleError.ts";
import useUpdateCategory from "../../hooks/categories/useUpdateCategory.ts";
import useCategories from "../../hooks/categories/useCategories.ts";
import useUserStore from "../../stores/userStore.ts";
import Spinner from "../../components/other/Spinner.tsx";
import * as Icons from "react-icons/md";
import {IconType} from "react-icons";
import {BsFillGearFill} from "react-icons/bs";
import {FaTrash} from "react-icons/fa6";
import useDeleteCategory from "../../hooks/categories/useDeleteCategory.ts";
import {HiX} from "react-icons/hi";

export default function WalletCategoriesSettingsPage() {

    // MdBrightness1 - for colour
    // <MdHub /> - for category
    const userId = useUserStore(x => x.getUserId());
    const {
        register,
        handleSubmit,
        setValue,
        formState: {errors},
        clearErrors,
        setError,
        getValues,
        reset
    } = useForm<Category>({
        mode: 'onSubmit'
    });

    const [categoryToCreate, setCategoryToCreate] = useState<Category>({
        iconColour: 'gray',
        name: "",
        image: "MdHub",
        type: CategoryType.EXPENSE,
        id: ""
    } as Category)
    const [categoryToEdit, setCategoryToEdit] = useState<Category | null>(null);

    const [categooryColorToCreate, setCategoryColorToCreate] = useState<Category>({
        iconColour: 'gray',
        name: "",
        image: "MdBrightness1",
        type: CategoryType.EXPENSE,
        id: ""
    } as Category)


    const [allCategories, setAllCategories] = useState<Category[]>(icons.map((icon, i) => ({
            type: CategoryType.EXPENSE,
            name: "",
            iconColour: 'gray',
            image: icon,
            id: i.toString()
        } as Category))
    )
    const [isEditing, setIsEditing] = useState(false);
    const {data: categories} = useCategories(userId!);
    const formRef = useRef<HTMLFormElement>(null);
    const nameRef = useRef<HTMLInputElement>(null);
    const submitButtonRef = useRef<HTMLButtonElement>(null);
    const [isDeleting, setIsDeleting] = useState(false);
    const [categoryToDelete, setCategoryToDelete] = useState<Category>();

    const categoryCreateMutation = useCreateCategory();
    const categoryUpdateMutation = useUpdateCategory();

    useEffect(() => {
        if (categoryToEdit) {
            setCategoryToCreate(categoryToEdit);
            setCategoryColorToCreate({iconColour: categoryToEdit.iconColour, image: "MdBrightness1"} as Category);
        }
    }, [categoryToEdit]);

    if (!categories) return <Spinner/>

    const expenseCategories = categories.filter(c => c.type === CategoryType.EXPENSE);
    const incomeCategories = categories.filter(c => c.type === CategoryType.INCOME);
    const categoryColors: Category[] = colors.map((color, i) => ({
        type: CategoryType.EXPENSE,
        name: "",
        iconColour: color,
        image: 'MdBrightness1',
        id: i.toString()
    } as Category));


    const handleSelectedCategory = (cat: Category) => setCategoryToCreate(cat);
    const handleSelectedIconColor = (cat: Category) => {
        setCategoryColorToCreate(cat);
        setCategoryToCreate(p => ({...p, iconColour: cat.iconColour}));
        setCategoryToCreate(p => ({...p, type: cat.type}));

        // Оновлюємо колір кожної категорії в allCategories
        let updatedCategories = allCategories.map(c => ({...c, iconColour: cat.iconColour}));

        // Встановлюємо оновлений масив як новий стан
        setAllCategories(updatedCategories);
    }

    const setDefaultValues = () => {
        setCategoryColorToCreate({iconColour: 'gray', image: 'MdBrightness1'} as Category);
        setCategoryToCreate({
            iconColour: 'gray',
            image: 'MdHub',
        } as Category)

        if (submitButtonRef.current)
            submitButtonRef.current.innerText = 'Create'

        reset()
    }
    const onSubmit: SubmitHandler<Category> = async (model: Category) => {
        model.image = categoryToCreate.image;
        model.iconColour = categoryToCreate.iconColour;
        model.type = +getValues('type');
        model.name = getValues('name');

        console.log("model: ", model);

        let result;
        if (isEditing) {
            model.id = categoryToCreate.id;
            console.log("model: ", model);
            result = await categoryUpdateMutation.mutateAsync(model);
        } else {
            result = await categoryCreateMutation.mutateAsync(model);
        }

        if (!result.hasError) {
            clearErrors();
            reset();
            setIsEditing(false)// Reset the editing state
            setDefaultValues()
        } else {
            handleServerErrorResponse(result.error, setError);
        }
    };

    const toggleIsEditing = () => {
        setIsEditing(p => !p);
        submitButtonRef.current!.innerText = 'Create';

        const newCat: Category = {
            iconColour: 'gray',
            image: 'MdHub',
        } as Category
        setCategoryToEdit(newCat)
        clearErrors();
    }

    const toggleDeleteModal = () => setIsDeleting(p => !p);
    const onTrashClick = async (cat: Category) => {
        toggleDeleteModal();
        setCategoryToDelete(cat);
    }

    const onGearClick = (cat: Category) => {
        clearErrors();
        const newCat: Category = {
            iconColour: cat.iconColour,
            image: cat.image,
            id: cat.id
        } as Category
        setCategoryToEdit(newCat)
        setIsEditing(true);

        setValue("name", cat.name)
        setValue("type", cat.type)

        if (nameRef.current)
            nameRef.current.value = cat.name

        if (submitButtonRef.current)
            submitButtonRef.current.innerText = 'Update'

        if (formRef && formRef.current)
            formRef.current.scrollIntoView({behavior: 'smooth', block: 'center'})
    }

    return (
        <div className={'mx-auto px-10 sm:px-12 md:px-24 lg:px-36 py-7 flex flex-col gap-y-10'}>
            <h1 className={'text-lg font-semibold'}>Create your new category</h1>
            <section>
                <form onSubmit={handleSubmit(onSubmit)} ref={formRef}>
                    <div className={'grid grid-cols-1 sm:grid-cols-2 gap-3'}>
                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2"
                            >Icon</label>
                            <div {...register("image")}>
                                <SingleSelectDropDownMenu items={allCategories} ItemComponent={CategoryItem}
                                                          heading={"Icons"} onItemSelected={handleSelectedCategory}
                                                          defaultSelectedItem={categoryToCreate}/>
                                {errors.image &&
                                    <p className={'text-red-400 italic'}>{errors.image.message}</p>}
                            </div>
                        </div>

                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2"
                            >Color</label>
                            <div {...register("iconColour")}>
                                <SingleSelectDropDownMenu items={categoryColors} ItemComponent={CategoryItem}
                                                          heading={"Colors"} onItemSelected={handleSelectedIconColor}
                                                          defaultSelectedItem={categooryColorToCreate}/>
                                {errors.image &&
                                    <p className={'text-red-400 italic'}>{errors.image.message}</p>}
                            </div>
                        </div>
                        <div className="sm:col-span-2">
                            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="name">
                                Name
                            </label>
                            <input
                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                id="name"
                                type="text"
                                {...register("name", nameRegisterOptionsForCategory)}
                                onChange={e => setValue("name", e.target.value)}
                                ref={nameRef}
                            />
                            {errors.name && <p className={'text-red-400 italic'}>{errors.name.message}</p>}
                        </div>
                        {!isEditing && <div className="sm:col-span-2">
                            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="type">
                                Category type
                            </label>
                            <select
                                {...register("type", {required: "Type for category is required"})}
                                defaultValue=""
                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                onChange={e => setValue("type", +e.target.value)}
                                id="type">

                                <option value=""></option>
                                <option value={CategoryType.EXPENSE}>Expense</option>
                                <option value={CategoryType.INCOME}>Income</option>
                            </select>

                            {errors.type && <p className={'text-red-400 italic'}>{errors.type.message}</p>}
                        </div>}
                        <div className={'flex gap-x-2 col-span-2'}>
                            <div className={'w-full bg-green-400 text-white font-semibold text-center rounded-lg mt-5'}>
                                <button ref={submitButtonRef} type={'submit'} className={'w-full  p-2 '}>Create</button>
                            </div>
                            {isEditing && <div
                                className={'w-full bg-red-400 text-white font-semibold text-center rounded-lg mt-5'}>
                                <button type={'submit'} onClick={() => {
                                    toggleIsEditing()
                                    setDefaultValues()
                                }}
                                        className={'w-full  p-2 '}>Cancel
                                </button>
                            </div>}
                        </div>
                    </div>
                </form>
            </section>
            <section className={'flex flex-col gap-y-5'}>
                <h2 className={'font-semibold mt-10'}>Manage your categories</h2>
                <div className={'mt-5'}>
                    <fieldset className={'flex flex-col gap-y-3'}>
                        <legend className={'font-semibold mb-5'}>Income categories</legend>
                        {incomeCategories.map(c =>
                            <div className={'flex justify-between items-center'} key={c.id || "Hi-hi-ha-ha"}>
                                <CategoryBlock category={c}/>
                                <CategoryCRUDButtons category={c} onTrashClick={onTrashClick}
                                                     onGearClick={onGearClick}/>
                            </div>
                        )}
                    </fieldset>
                </div>
                <div className={'mt-5'}>
                    <fieldset className={'flex flex-col gap-y-3'}>
                        <legend className={'font-semibold mb-5'}>Expense categories</legend>
                        {expenseCategories.map(c =>
                            <div className={'flex justify-between items-center'} key={c.id}>
                                <CategoryBlock category={c}/>
                                <CategoryCRUDButtons category={c} onTrashClick={onTrashClick}
                                                     onGearClick={onGearClick}/>
                            </div>
                        )}
                    </fieldset>
                </div>
            </section>
            {isDeleting && <DeleteCategoryModal categoryToDelete={categoryToDelete!} allCategories={categories}
                                                toggleModal={toggleDeleteModal}
                                                handleSelectedCategory={(cat) => setCategoryToDelete(cat)}/>}
        </div>
    )
}

interface DeleteCategoryModalProps {
    categoryToDelete: Category;
    allCategories: Category[];
    toggleModal: () => void;
    handleSelectedCategory: (cat: Category | undefined) => void;
}

export function DeleteCategoryModal({
                                        categoryToDelete,
                                        allCategories,
                                        toggleModal,
                                        handleSelectedCategory,
                                    }: DeleteCategoryModalProps) {
    const [isForceDelete, setIsForseDelete] = useState(false);
    const categoryDeleteMutation = useDeleteCategory();
    const [categoryToReplace, setCategoryToReplace] = useState<Category>(categoryToDelete);
    const toggleForceDelete = () => setIsForseDelete(p => !p);
    const handleDelete = async () => {
        
        console.log("categoryToReplace: ", categoryToReplace)
        console.log("categoryToDelete: ", categoryToDelete)
    
        console.log({
            categoryToReplaceId: categoryToReplace!.id,
            id: categoryToDelete!.id,
            shouldReplace: isForceDelete
        })
        
        await categoryDeleteMutation.mutateAsync({
            categoryToReplaceId: categoryToReplace!.id,
            id: categoryToDelete!.id,
            shouldReplace: isForceDelete
        })
    }

    return (
        <div
            className={'absolute inset-0 flex justify-center items-center px-4 lg:px-0 visible bg-black/20 z-50'}>
            <div className="bg-white p-4 rounded-md shadow-lg max-w-full mx-auto mt-4">
                <h2 className="text-2xl font-bold mb-4 flex justify-between">Category deletion
                    <HiX size={'2rem'} color={'red'} onClick={() => {
                        toggleModal()
                        handleSelectedCategory(undefined)
                    }}/>
                </h2>
                <div className={'w-full flex flex-col text-lg gap-y-5'}>
                    <p>
                        You category is used in {categoryToDelete?.transactionCount} transactions
                    </p>
                    <p>
                        You category is used in {categoryToDelete?.budgetCount} budgets
                    </p>
                    <div className={'flex gap-10'}>
                        <p>
                            Would you like to replace?
                        </p>
                        <div className="flex gap-4">
                            <div className="relative">
                                <input
                                    onChange={toggleForceDelete}
                                    className="h-6 w-6 border-blue-500 rounded-full cursor-pointer checked:border-transparent 
                                        checked:bg-blue-500"
                                    id="isPublic"
                                    type="checkbox"
                                />
                            </div>
                        </div>
                    </div>
                    {isForceDelete && <div>
                        <SingleSelectDropDownMenu items={allCategories} ItemComponent={CategoryItem}
                                                  heading={"Categories"}
                                                  onItemSelected={(cat) => setCategoryToReplace(cat)}
                                                  defaultSelectedItem={categoryToReplace}/>
                    </div>}
                </div>
                <div className={'flex justify-center items-center mt-5'}>
                    <button 
                        onClick={handleDelete}
                        className={'bg-red-400 px-4 py-2 rounded text-white'}>Delete</button>
                </div>
            </div>
        </div>
    )
}

interface CategoryBlockProps {
    category: Category;
}

export function CategoryBlock({category}: CategoryBlockProps) {
    const Icon = (Icons as any)[category.image] as IconType;

    return (
        <div className={'flex items-center h-auto w-full'}>
            <Icon color={category.iconColour} className="" size={'2.5rem'}/>
            <p className="ml-3 flex flex-col">
                <span className={'text-sm font-semibold text-gray-700'}>{category.name}</span>
                <span className={'text-[14px] text-gray-500'}>{category.transactionCount} transactions</span>
            </p>
        </div>
    )
}

interface CategoryCRUDButtonsProps {
    category: Category;
    onGearClick: (cat: Category) => void;
    onTrashClick: (cat: Category) => void;
}

export function CategoryCRUDButtons({category, onGearClick, onTrashClick}: CategoryCRUDButtonsProps) {

    return (
        <div className={'flex items-center justify-center gap-x-3'}>
            <div className={'bg-green-400/30 p-2 rounded hover:bg-green-300/70 transition-all transform duration-300'}>
                <button
                    type={'button'}
                    onClick={() => onGearClick(category)}
                    className={'text-center flex justify-center items-center'}>
                    <BsFillGearFill size={'1rem'} color={'rgb(74 222 128 / 1)'}/>
                </button>
            </div>
            <div className={'bg-red-300/40 p-2 rounded hover:bg-red-300/70 transition-all transform duration-300'}>
                <button
                    type={'button'}
                    onClick={() => onTrashClick(category)}
                    className={'text-center flex justify-center items-center'}>
                    <FaTrash size={'1rem'} color={'red'}/>
                </button>
            </div>

        </div>
    )
}