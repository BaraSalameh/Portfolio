'use client';

import { InputHTMLAttributes } from 'react';

interface CardProps extends InputHTMLAttributes<HTMLInputElement> {
    title?: string;
    description?: string;
    label?: string;
}

export const Card = ({
    title,
    description,
    label,
    ...rest
}: CardProps) => {
    return (
        <div className="p-6 border rounded-2xl shadow hover:shadow-lg transition">
            <h2 className="text-xl font-semibold mb-2">{title}</h2>
            <p className="text-gray-600 mb-4">{description}</p>
            <button className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">
                {label}
            </button>
        </div>
    );
};
