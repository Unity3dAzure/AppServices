namespace Unity3dAzure.AppServices
{
	/// <summary>
	/// Interface to support table Query with `$inlinecount=allpages` 
	/// </summary>
	public interface INestedResults
	{
	}

    public interface INestedResults<T>
    {
        // work-around for WSA
        string GetArrayField();
        string GetCountField();

        void SetArray(T[] array);
        void SetCount(uint count);
        
    }
}