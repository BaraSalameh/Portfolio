import { PieChart, Pie, Cell, Tooltip, BarChart, Bar, XAxis, YAxis, ResponsiveContainer } from 'recharts';
import { Briefcase, Clock, MapPin } from 'lucide-react';
import dayjs from 'dayjs';
import { Paragraph } from './Paragraph';
import ResponsiveIcon from './ResponsiveIcon';
import { Header } from '../shared/Header';
import { Main } from '../shared/Main';
import { List } from './List';
import { CustomTooltip } from '../utils/customTooltip';

const COLORS = ['#F97316', '#3B82F6', '#10B981', '#EAB308', '#6366F1'];

export function ExperienceCard({ lstExperiences }: { lstExperiences: any }) {
    if (!Array.isArray(lstExperiences) || lstExperiences.length === 0) return null;

    // Pie data: Job title distribution
    const jobTitleCounts = lstExperiences.reduce((acc, exp) => {
        acc[exp.jobTitle] = (acc[exp.jobTitle] || 0) + 1;
        return acc;
    }, {});
    const pieData = Object.entries(jobTitleCounts).map(([name, value]) => ({ name, value }));

    // Bar data: Duration per experience
    const durationData = lstExperiences.map(exp => {
        const start = dayjs(exp.startDate);
        const end = exp.endDate ? dayjs(exp.endDate) : dayjs();
        return {
            name: exp.jobTitle,
            duration: end.diff(start, 'month'),
        };
    });

    const colorMap = pieData.reduce((acc, item, index) => {
        acc[item.name] = COLORS[index % COLORS.length];
        return acc;
    }, {} as Record<string, string>);

    return (
        <section className="bg-green-900 p-4 rounded-2xl">
            <Header paddingX='xs' paddingY='xs' space='sm'>
                <ResponsiveIcon icon={Briefcase} />
                <Paragraph size='lg'>Experience</Paragraph>
            </Header>

            <Main paddingX='none' paddingY='md'>
                <div className="grid grid-cols-1 sm:grid-cols-3 w-full">
                    {/* Pie Chart: Job Titles */}
                    <div className="h-64">
                        <Paragraph size='md' position='center'>Job Titles Overview</Paragraph>
                        <ResponsiveContainer>
                            <PieChart>
                                <Pie
                                    data={pieData}
                                    dataKey="value"
                                    nameKey="name"
                                    cx="50%"
                                    cy="50%"
                                    outerRadius={50}
                                    label
                                >
                                    {pieData.map((entry, index) => (
                                        <Cell key={`cell-${index}`} fill={colorMap[entry.name]} />
                                    ))}
                                </Pie>
                                <Tooltip content={CustomTooltip} />
                            </PieChart>
                        </ResponsiveContainer>
                    </div>

                    {/* Bar Chart: Duration */}
                    <div className="h-64 col-span-2 space-y-3">
                        <Paragraph size='md' position='center'>Duration (in months)</Paragraph>
                        <ResponsiveContainer width="100%" height="100%">
                            <BarChart data={durationData}>
                                <XAxis dataKey="name" />
                                <YAxis />
                                <Tooltip content={CustomTooltip} />
                                <Bar
                                    dataKey="duration"
                                    radius={[10, 0, 10, 0]}
                                    fillOpacity={1}
                                >
                                    {durationData.map((entry, index) => (
                                        <Cell key={`cell-bar-${index}`} fill={colorMap[entry.name]} />
                                    ))}
                                </Bar>
                            </BarChart>
                        </ResponsiveContainer>
                    </div>
                </div>
            </Main>

            {/* List of Experiences */}
            <Main paddingX='none' paddingY='none'>
                <List size='md' as='none' className='w-full'>
                    {lstExperiences.map(exp => (
                        <li key={exp.id} className="bg-green-700 p-4 rounded-xl shadow-sm space-y-2">
                            <Paragraph size='lg'>{exp.jobTitle} at {exp.companyName}</Paragraph>
                            <Paragraph className='text-green-200 flex gap-2 items-center'>
                                <ResponsiveIcon icon={MapPin} />
                                {exp.location}
                            </Paragraph>
                            <Paragraph className='text-green-200 flex gap-2 items-center'>
                                <ResponsiveIcon icon={Clock} />
                                {dayjs(exp.startDate).format('MMM YYYY')} – {exp.endDate ? dayjs(exp.endDate).format('MMM YYYY') : 'Present'}
                            </Paragraph>
                        </li>
                    ))}
                </List>
            </Main>
        </section>
    );
}
