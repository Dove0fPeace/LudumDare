namespace DefaultNamespace
{
    public static class StoryContainer
    {
        public static int storyPhase = 0;
        
        private static string[][] EngStories = new string[][]
        {
            new []
            {
                "Awake and break out of your cocoon, little one. You were made to break the will of disobedient people and put them on their knees.",
                "Just like your brothers and sisters,  you are the best of my creations. And still, you are far from perfect.",
                "Prove the opposite to your Master! Kill your brothers and sisters in the arena and soak up their essence. Become the apex predator!",
                "..."
            },
            new []
            {
                "— Do-o-o-es this o-o-o-ne speak?",
                "...",
                "— I am already bet-ter than youu, and Massster seesss it. Pre-pare to die, filtth."
            },
            new []
            {
            "— How many have you already killed?",
            "— Juuust three-e of themm",
            "— Weak one, I've killed four already. I managed to split my sister's belly while I was waiting for this fight. My father was very pleased with me!",
            "— Cleverr...",
            "— Let the battle begin!"
            },
            new []
            {
                "What a wonderful creation you are! I can see your power. Kill the rest of them, take from them what is yours.",
                "After all, you are the one I believe in and the one who will defeat the champion of the people. You may then feast, absorb the defiant and break their will.",
                "— How many have you killed?",
                "— You won't be the final one..."
            },
            new []
            {
                "It’s been a long time since I’ve enjoyed watching someone fight like this. Usually there’s no one to fight, because I eat everyone. But you’re perfect!",
                "The essence of pure primitive cruelty, cunning, and endless will to live. Dopamine overflows in me with the thought of your devotion, in your desire to serve you have cut down all of your kin.",
                "Although, there is one left. And I will not tolerate defeat.",
                "— Any last words?",
                "— LAST WORDS?! How dare you behave so brazenly with the ppppperfect creation?",
                "...",
                "— Brainless piece of meat. Your essence belongs to me. I’ll rip you apart, bastard."
            },
            new []
            {
                "It is so amusing to watch those who walk the path of mine. Go now to the exit of the arena, I will lead you to the people..."
            }
        };
        
        private static string[][] Stories = new string[][]
        {
            new []
            {
                "— Воспрянь ото сна, разорви свой кокон. Я создал тебя, чтобы добить, сломить волю непокорных людишек. Ты, как и твои братья и сёстры, лучшие из моих творений. Но не идеальные. Докажи своему великодушному создателю обратное! Убей своих братьев и сестёр на арене и впитай их эссенцию. Стань идеальным убийцей!",
                "..."
            },
            new []
            {
                "— Ты-ы-ы умееш-ш-шь го-о-оворить?",
                "— Я уж-ж-же лучш-ш-ш-ше тебя ис-сполняю во-олю с-с-создателя. Умр-р-ри, ни-и-икчёмнос-с-сть."
            },
            new []
            {
            "— Скольких ты уже убил?",
            "— Ли-и-ишь трех-х.",
            "— Слабо, я уже трёх. Пока ждал боя с тобой, успел распороть брюхо сестре. Отец был очень доволен мною!",
            "— Хитр-р-рый.",
            "— А теперь же пусть победит сильнейший!"
            },
            new []
            {
                "— Какое замечательное творение! Я вижу твою мощь. Добей оставшихся, отбери у них то, что принадлежит тебе. Ведь именно в тебя я верю и именно тебе нужно победить чемпиона людей. Затем ты сможешь вволю насытиться, поглотить непокорных и сломать их волю.",
                "— Скольких ты уже убил?",
                "— Ты не последний..."
            },
            new []
            {
                "Давно я так не наслаждался, наблюдая за чьей-то битвой. Обычно некому сражаться, ведь я всех пожираю. Но ты идеален! Эссенция чистейшей первобытной жестокости, коварной хитрости и бесконечной воли к жизни. Дофамин переполняет лишь от одной мысли, что в своей преданности, в своём желании испольнить мою волю, ты вырезал всю свою родню. Хотя нет, остался последний, на сладкое. Поражения я не потерплю.",
                "Последнее слово?",
                "ПОСЛЕДНЕЕ СЛОВО?! Как ты смеешь так нагло вести себя с идеальным творением?",
                "...",
                "Безмозглый кусок мясо, скрывающий мою эссенцию. Выпотрошу, выродок."
            },
            new []
            {
                "Так интересно наблюдать за тем, кто проходит мой же путь. Иди на выход из арены, я выведу тебя этим тоннелем к людям."
            }
        };

        public static string[] GetStory(int num)
        {
            storyPhase = num;
            return EngStories[num];
        }

        public static int GetEnemiesCount(int num)
        {
            switch (num)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                default:
                    return 3;
            }
        }

        public static int GetEnemiesHp(int num)
        {
            switch (num)
            {
                case 0:
                    return 30;
                case 1:
                    return 25;
                case 2:
                    return 20;
                case 3:
                    return 30;
                case 4:
                    return 40;
            }

            return 30;
        }

        public static float GetEnemiesBrain(int num)
        {
            switch (num)
            {
                case 0:
                    return 2;
                case 1:
                    return 2;
                case 2:
                    return 3;
                case 3:
                    return 4;
                case 4:
                    return 5;
            }

            return 3;
        }
    }
}