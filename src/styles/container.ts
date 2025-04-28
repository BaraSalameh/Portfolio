import { cva, VariantProps } from 'class-variance-authority';

export const container = cva(
    'flex-grow grid grid-rows-[100px_1fr_100px] font-[family-name:var(--font-geist-sans)]'
);

export type ContainerVariantProps = VariantProps<typeof container>;
