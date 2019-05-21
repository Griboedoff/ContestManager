import { UserRole } from './Enums/UserRole';

export const roleToString = {
    [UserRole.Participant]: "Участник",
    [UserRole.Coach]: "Тренер",
    [UserRole.Volunteer]: "Волонтер",
    [UserRole.Checker]: "Проверяющий",
    [UserRole.Admin]: "Админ",
};

export const sexToString = {
    0: "М",
    1: "Ж",
};


