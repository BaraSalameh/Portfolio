'use client';

import Select, { ActionMeta, MultiValue, SingleValue } from 'react-select';
import { FieldError } from 'react-hook-form';
import { Paragraph } from './Paragraph';
import { Option } from '../types';


interface FormDropdownProps {
    label?: string;
    options: Option[];
    value?: Option | MultiValue<Option>;
    onChange?: (value: MultiValue<Option> | SingleValue<Option>, actionMeta: ActionMeta<Option>) => void;
    onBlur?: () => void;
    error?: FieldError;
    isSearchable?: boolean;
    isClearable?: boolean;
    isMulti?: boolean;
    placeholder?: string;
    isLoading?: boolean;
}

export const FormDropdown = ({
    label,
    options,
    value,
    onChange,
    onBlur,
    error,
    isSearchable = true,
    isClearable = true,
    isMulti = false,
    placeholder = 'Select...',
    isLoading
}: FormDropdownProps) => {
    const customStyles = {
        control: (provided: any, state: any) => ({
            ...provided,
            width: '100%',
            padding: '0.5rem 1rem', // Tailwind: px-4 py-2
            marginTop: '0.25rem', // Tailwind: mt-1
            borderWidth: '1px',
            borderRadius: '0.5rem', // Tailwind: rounded-lg
            borderColor: error ? '#f87171' : '#6b7280', // border-red-500 or border-gray-500
            backgroundColor: 'transparent', // 👈 make background transparent
            outline: 'none',
            boxShadow: state.isFocused
                ? `0 0 0 2px ${error ? '#f87171' : '#6b7280'}` // focus:ring-2 color mimic
                : undefined,
            '&:hover': {
                borderColor: error ? '#f87171' : '#6b7280',
            },
        }),
        menu: (provided: any) => ({
            ...provided,
            zIndex: 10,
            backgroundColor: '#15803d', // or keep it white for dropdown options
            borderRadius: '0.5rem', // Tailwind: rounded-lg
        }),
        singleValue: (provided: any) => ({
            ...provided,
            color: 'inherit', // inherits from parent, works well with transparent bg
        }),
        input: (provided: any) => ({
            ...provided,
            color: 'inherit',
            cursor: 'text',
        }),
        placeholder: (provided: any) => ({
            ...provided,
            color: '#9ca3af', // Tailwind text-gray-400
        }),
        option: (provided: any, state: any) => ({
            ...provided,
            backgroundColor: state.isSelected
                ? '#22c55e' // green-700 for selected item
                : state.isFocused
                ? '#22c55e' // green-500 on hover/focus
                : 'transparent',
            color: state.isSelected || state.isFocused ? 'white' : 'inherit',
            cursor: 'pointer',
        }),
    };



    return (
        <div className="space-y-1">
            {label && (
                <label className="block text-sm font-medium text-white-700">
                    <Paragraph>{label}</Paragraph>
                </label>
            )}
            <Select
                options={options}
                value={value}
                onChange={onChange}
                styles={customStyles}
                isSearchable={isSearchable}
                isClearable={isClearable}
                isMulti={isMulti}
                placeholder={placeholder}
                onBlur={onBlur}
                isLoading={isLoading}
            />
            {error && <Paragraph intent="danger" size="sm">{error.message}</Paragraph>}
        </div>
    );
};
