namespace LastHost.Prototype.Mutations
{
    public static class MutationDefinition
    {
        public static string GetName(MutationType type)
        {
            switch (type)
            {
                case MutationType.Dormancy:
                    return "잠복 강화";
                case MutationType.NeuralControl:
                    return "신경 조종";
                case MutationType.MammalAdaptation:
                    return "포유류 적응";
                default:
                    return type.ToString();
            }
        }

        public static string GetPrototypeEffect(MutationType type)
        {
            switch (type)
            {
                case MutationType.Dormancy:
                    return "면역 경계도 상승 속도 감소";
                case MutationType.NeuralControl:
                    return "쥐 이동 속도 증가";
                case MutationType.MammalAdaptation:
                    return "하수도 특정 통로 접근 허용";
                default:
                    return string.Empty;
            }
        }
    }
}
