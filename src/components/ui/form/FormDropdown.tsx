'use client';

import Select from 'react-select';
import { FormDropdownProps } from './types';
import { Paragraph } from '../Paragraph';

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
        multiValue: (provided: any) => ({
            ...provided,
            backgroundColor: '#22c55e', // Tailwind: green-500
            borderRadius: '0.375rem', // Tailwind: rounded-md
            padding: '2px 6px',
        }),

        multiValueLabel: (provided: any) => ({
            ...provided,
            color: 'white',
            // fontWeight: 500,
        }),

        multiValueRemove: (provided: any) => ({
            ...provided,
            color: 'white',
            cursor: 'pointer',
            ':hover': {
                backgroundColor: '#15803d', // Tailwind: green-700
                color: 'white',
            },
        }),
        clearIndicator: (provided: any, state: any) => ({
            ...provided,
            color: 'white',
            cursor: 'pointer'
        }),
        dropdownIndicator: (provided: any, state: any) => ({
            ...provided,
            color: 'white',
            cursor: 'pointer',
            transition: 'transform 0.2s ease',
            transform: state.selectProps.menuIsOpen ? 'rotate(180deg)' : 'rotate(0deg)'
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
                closeMenuOnSelect={isMulti ? false : true}
            />
            {error && <Paragraph intent="danger" size="sm">{error.message}</Paragraph>}
        </div>
    );
};
