namespace Faker
{
    public interface IPersonProvider : IProvider<string>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        string Name();
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        string FirstName();
        /// <summary>
        /// 姓氏
        /// </summary>
        /// <returns></returns>
        string LastName();
        /// <summary>
        /// 男性名称
        /// </summary>
        /// <returns></returns>
        string NameMale();
        /// <summary>
        /// 女性名称
        /// </summary>
        /// <returns></returns>
        string NameFemale();

        /// <summary>
        /// 罗马拼音
        /// </summary>
        /// <returns></returns>
        string RomanizedName();

    }
}