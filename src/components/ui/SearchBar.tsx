'use client';

import { useState, useCallback } from 'react';
import { Search as SearchIcon } from 'lucide-react';
import { useAppDispatch, useAppSelector } from '@/lib/store/hooks';
import { userListQuery } from '@/lib/apis/client/userListQuery';
import debounce from 'lodash.debounce';
import { Paragraph } from './Paragraph';
import { useRouter } from 'next/navigation';

export const SearchBar = () => {
    const [query, setQuery] = useState('');
    const dispatch = useAppDispatch();
    const router = useRouter();
    const { userList, loading, error } = useAppSelector(state => state.search);

    const debouncedSearch = useCallback(
        debounce((value: string) => {
        if (value.trim().length > 0) {
            dispatch(userListQuery(value));
        }
        }, 300), []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setQuery(value);
        debouncedSearch(value);
    };

    const handleSelect = (user: any) => {
        router.push(`/client/${user.username}/about`);
    };

    return (
        <div className="relative max-w-md mx-auto">
            <form
                className="
                    flex
                    items-center
                    bg-green-900
                    dark:bg-zinc-900
                    border
                    border-zinc-300
                    dark:border-green-900
                    rounded-full
                    px-4
                    py-2
                    shadow-sm
                "
            >
                <SearchIcon className="text-gray-500 w-5 h-5 mr-2" />
                <input
                    type="text"
                    value={query}
                    onChange={handleChange}
                    placeholder="Search for a user"
                    className="flex-grow bg-transparent outline-none text-sm text-black dark:text-white"
                />
            </form>

            {/* Dropdown List */}
            {query && (
                <div 
                    className="
                        absolute
                        w-full
                        mt-1
                        z-10
                        bg-white
                        dark:bg-zinc-900
                        border
                        border-gray-300
                        dark:border-zinc-700
                        rounded-md
                        shadow-lg
                        max-h-60
                        overflow-y-auto
                    "
                >
                    {error
                        ? <Paragraph intent="danger" size="sm" className='p-3'>{error}</Paragraph> 
                        : loading
                            ? <Paragraph size="sm" className='p-3'>Loading...</Paragraph>
                            : userList?.length === 0 && <Paragraph size="sm" className='p-3'>No Result</Paragraph>
                    }
                    
                    {userList?.map((user: any, id: number) => (
                        <div
                            key={id}
                            onClick={() => handleSelect(user)}
                            className="flex gap-3 p-3 hover:bg-gray-100 dark:hover:bg-zinc-800 cursor-pointer"
                        >
                            <div className="font-medium text-black dark:text-white">{user.profilePicture}</div>
                            <Paragraph size="md">{user.firstname} {user.lastname}</Paragraph>
                            <Paragraph size="xs">({user.title})</Paragraph>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};
